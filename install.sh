#!/usr/bin/env bash

set -Eeuo pipefail

INSTALL_DIR="/srv/Archivyn"
COMPOSE_FILE="$INSTALL_DIR/compose.yaml"
ENV_FILE="$INSTALL_DIR/.env"
DATABASE_VOLUME="archivyn_archivyn_postgres_data"

# Allow prompts when executed with:
# curl .../install.sh | sudo bash
exec 3</dev/tty

if [[ "$EUID" -ne 0 ]]; then
    echo "Run the installer with sudo."
    exit 1
fi

if ! command -v docker >/dev/null 2>&1; then
    echo "Docker is not installed."
    exit 1
fi

if ! docker compose version >/dev/null 2>&1; then
    echo "Docker Compose is not installed."
    exit 1
fi

INSTALL_USER="${SUDO_USER:-root}"
INSTALL_GROUP="$(id -gn "$INSTALL_USER")"

echo
echo "Archivyn Installer"
echo

install \
    -d \
    -m 0750 \
    -o "$INSTALL_USER" \
    -g "$INSTALL_GROUP" \
    "$INSTALL_DIR"

# Refuse to create new credentials for an existing database.
if [[ ! -f "$ENV_FILE" ]] &&
   docker volume inspect "$DATABASE_VOLUME" >/dev/null 2>&1
then
    echo "Installation stopped."
    echo
    echo "An existing Archivyn database was found, but:"
    echo "$ENV_FILE"
    echo "is missing."
    echo
    echo "Restore the original .env file before continuing."
    exit 1
fi

# Only ask questions during the first installation.
if [[ ! -f "$ENV_FILE" ]]; then
    while true; do
        read -r -u 3 -p \
            "Archivyn web port [7421]: " \
            ARCHIVYN_PORT

        ARCHIVYN_PORT="${ARCHIVYN_PORT:-7421}"

        if [[ "$ARCHIVYN_PORT" =~ ^[0-9]+$ ]] &&
           (( ARCHIVYN_PORT >= 1 && ARCHIVYN_PORT <= 65535 ))
        then
            break
        fi

        echo "Enter a port between 1 and 65535."
    done

    while true; do
        read -r -u 3 -p \
            "Database name [archivyn]: " \
            POSTGRES_DB

        POSTGRES_DB="${POSTGRES_DB:-archivyn}"

        if [[ "$POSTGRES_DB" =~ ^[a-z_][a-z0-9_]*$ ]]; then
            break
        fi

        echo "Use lowercase letters, numbers, and underscores only."
    done

    while true; do
        read -r -u 3 -p \
            "Database user [archivyn]: " \
            POSTGRES_USER

        POSTGRES_USER="${POSTGRES_USER:-archivyn}"

        if [[ "$POSTGRES_USER" =~ ^[a-z_][a-z0-9_]*$ ]]; then
            break
        fi

        echo "Use lowercase letters, numbers, and underscores only."
    done

    if command -v openssl >/dev/null 2>&1; then
        POSTGRES_PASSWORD="$(openssl rand -hex 32)"
    else
        POSTGRES_PASSWORD="$(
            head -c 32 /dev/urandom |
            od -An -tx1 |
            tr -d ' \n'
        )"
    fi

    umask 077

    cat > "$ENV_FILE" <<ENV
ARCHIVYN_VERSION=latest
ARCHIVYN_PORT=${ARCHIVYN_PORT}

POSTGRES_DB=${POSTGRES_DB}
POSTGRES_USER=${POSTGRES_USER}
POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
ENV

    chown "$INSTALL_USER:$INSTALL_GROUP" "$ENV_FILE"
    chmod 0600 "$ENV_FILE"

    unset POSTGRES_PASSWORD

    echo
    echo "Created a new database configuration."
    echo "A secure database password was generated automatically."
else
    echo "Existing configuration found."
    echo "The current database settings will be preserved."
fi

cat > "$COMPOSE_FILE" <<'COMPOSE'
name: archivyn

services:
  archivyn:
    image: ghcr.io/brentopc/archivyn:${ARCHIVYN_VERSION:-latest}
    container_name: archivyn
    restart: unless-stopped

    ports:
      - "${ARCHIVYN_PORT:-7421}:8080"

    environment:
      ASPNETCORE_ENVIRONMENT: Production
      ConnectionStrings__Archivyn: >-
        Host=postgres;
        Port=5432;
        Database=${POSTGRES_DB};
        Username=${POSTGRES_USER};
        Password=${POSTGRES_PASSWORD};

    volumes:
      - archivyn_data_protection:/root/.aspnet/DataProtection-Keys

    depends_on:
      postgres:
        condition: service_healthy

  postgres:
    image: postgres:17
    container_name: archivyn-postgres
    restart: unless-stopped

    environment:
      POSTGRES_DB: "${POSTGRES_DB}"
      POSTGRES_USER: "${POSTGRES_USER}"
      POSTGRES_PASSWORD: "${POSTGRES_PASSWORD}"

    volumes:
      - archivyn_postgres_data:/var/lib/postgresql/data

    healthcheck:
      test:
        - CMD-SHELL
        - pg_isready -U ${POSTGRES_USER} -d ${POSTGRES_DB}
      interval: 5s
      timeout: 5s
      retries: 10

volumes:
  archivyn_postgres_data:
  archivyn_data_protection:
COMPOSE

chown "$INSTALL_USER:$INSTALL_GROUP" "$COMPOSE_FILE"
chmod 0644 "$COMPOSE_FILE"

echo
echo "Validating configuration..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    config >/dev/null

echo "Pulling containers..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    pull

echo "Starting Archivyn..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    up -d \
    --remove-orphans

echo "Checking Archivyn..."

sleep 8

if [[ "$(docker inspect -f '{{.State.Running}}' archivyn 2>/dev/null || true)" != "true" ]]; then
    echo
    echo "Archivyn failed to start:"
    docker logs --tail=100 archivyn || true
    exit 1
fi

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    ps

SERVER_IP="$(hostname -I | awk '{print $1}')"

ARCHIVYN_PORT="$(
    grep '^ARCHIVYN_PORT=' "$ENV_FILE" |
    cut -d= -f2-
)"

echo
echo "Archivyn installation complete."
echo "Open: http://${SERVER_IP}:${ARCHIVYN_PORT}"
echo "Configuration files: $INSTALL_DIR"