#!/usr/bin/env bash

set -Eeuo pipefail

INSTALL_DIR="/srv/Archivyn"
COMPOSE_FILE="$INSTALL_DIR/compose.yaml"
ENV_FILE="$INSTALL_DIR/.env"

if [[ "${EUID}" -ne 0 ]]; then
    echo "Run this installer with sudo."
    exit 1
fi

echo "Installing Archivyn..."

if ! command -v docker >/dev/null 2>&1; then
    echo "Error: Docker is not installed."
    exit 1
fi

if ! docker compose version >/dev/null 2>&1; then
    echo "Error: Docker Compose is not installed."
    exit 1
fi

mkdir -p "$INSTALL_DIR"

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
        Database=${POSTGRES_DB:-archivyn};
        Username=${POSTGRES_USER:-archivyn};
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
      POSTGRES_DB: "${POSTGRES_DB:-archivyn}"
      POSTGRES_USER: "${POSTGRES_USER:-archivyn}"
      POSTGRES_PASSWORD: "${POSTGRES_PASSWORD}"

    volumes:
      - archivyn_postgres_data:/var/lib/postgresql/data

    healthcheck:
      test:
        - CMD-SHELL
        - pg_isready -U ${POSTGRES_USER:-archivyn} -d ${POSTGRES_DB:-archivyn}
      interval: 5s
      timeout: 5s
      retries: 10

volumes:
  archivyn_postgres_data:
  archivyn_data_protection:
COMPOSE

if [[ ! -f "$ENV_FILE" ]]; then
    if command -v openssl >/dev/null 2>&1; then
        DATABASE_PASSWORD="$(openssl rand -hex 32)"
    else
        DATABASE_PASSWORD="$(
            head -c 48 /dev/urandom |
            base64 |
            tr -d '\n'
        )"
    fi

    umask 077

    cat > "$ENV_FILE" <<ENV
ARCHIVYN_VERSION=latest
ARCHIVYN_PORT=7421

POSTGRES_DB=archivyn
POSTGRES_USER=archivyn
POSTGRES_PASSWORD=${DATABASE_PASSWORD}
ENV

    chmod 600 "$ENV_FILE"

    echo "Created a new database configuration."
else
    echo "Existing database configuration found."
    echo "The existing database password was preserved."
fi

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
    up -d

echo
docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    ps

echo
echo "Archivyn installation complete."
echo "Open: http://$(hostname -I | awk '{print $1}'):7421"
echo "Files: $INSTALL_DIR"
