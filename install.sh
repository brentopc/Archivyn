#!/usr/bin/env bash

set -Eeuo pipefail

INSTALL_DIR="/srv/Archivyn"
COMPOSE_FILE="$INSTALL_DIR/compose.yaml"
ENV_FILE="$INSTALL_DIR/.env"

# Database volume used by older Archivyn installations.
LEGACY_DATABASE_VOLUME="archivyn_archivyn_postgres_data"

# Allows prompts when executed with:
# curl -fsSL https://raw.githubusercontent.com/brentopc/Archivyn/main/install.sh | sudo bash
exec 3</dev/tty

fail() {
    echo
    echo "Installation stopped: $*" >&2
    exit 1
}

get_env_value() {
    local key="$1"

    [[ -f "$ENV_FILE" ]] || return 0

    awk -F= -v key="$key" '
        $1 == key {
            sub(/^[^=]*=/, "")
            print
            exit
        }
    ' "$ENV_FILE"
}

prompt_port() {
    local current_value="$1"
    local entered_value

    while true; do
        read -r -u 3 -p \
            "Archivyn web port [$current_value]: " \
            entered_value

        entered_value="${entered_value:-$current_value}"

        if [[ "$entered_value" =~ ^[0-9]+$ ]] &&
           (( entered_value >= 1 && entered_value <= 65535 ))
        then
            printf '%s' "$entered_value"
            return
        fi

        echo "Enter a port between 1 and 65535." >&2
    done
}

prompt_database_identifier() {
    local prompt_text="$1"
    local current_value="$2"
    local entered_value

    while true; do
        read -r -u 3 -p \
            "$prompt_text [$current_value]: " \
            entered_value

        entered_value="${entered_value:-$current_value}"

        if [[ "$entered_value" =~ ^[a-z_][a-z0-9_]*$ ]]; then
            printf '%s' "$entered_value"
            return
        fi

        echo \
            "Use lowercase letters, numbers, and underscores only." \
            >&2
    done
}

prompt_absolute_path() {
    local prompt_text="$1"
    local default_path="$2"
    local entered_path

    while true; do
        read -r -u 3 -p \
            "$prompt_text [$default_path]: " \
            entered_path

        entered_path="${entered_path:-$default_path}"

        if [[ "$entered_path" != /* ]]; then
            echo \
                "Enter an absolute path beginning with /." \
                >&2
            continue
        fi

        if [[ "$entered_path" == "/" ]]; then
            echo \
                "The filesystem root cannot be used as a storage directory." \
                >&2
            continue
        fi

        if [[ "$entered_path" == *:* ]]; then
            echo \
                "The path cannot contain a colon (:)." \
                >&2
            continue
        fi

        # Remove trailing slashes.
        while [[ "$entered_path" != "/" &&
                 "$entered_path" == */ ]]
        do
            entered_path="${entered_path%/}"
        done

        printf '%s' "$entered_path"
        return
    done
}

create_password() {
    if command -v openssl >/dev/null 2>&1; then
        openssl rand -hex 32
    else
        head -c 32 /dev/urandom |
            od -An -tx1 |
            tr -d ' \n'
    fi
}

if [[ "$EUID" -ne 0 ]]; then
    fail "run the installer with sudo."
fi

if ! command -v docker >/dev/null 2>&1; then
    fail "Docker is not installed."
fi

if ! docker compose version >/dev/null 2>&1; then
    fail "Docker Compose is not installed."
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

# Do not generate new credentials if an older database exists but its
# corresponding .env file is missing.
if [[ ! -f "$ENV_FILE" ]] &&
   docker volume inspect "$LEGACY_DATABASE_VOLUME" \
       >/dev/null 2>&1
then
    fail \
        "an existing Archivyn database was found, but $ENV_FILE is missing. Restore the original .env file before continuing."
fi

if [[ -f "$ENV_FILE" ]]; then
    echo "Existing configuration found."
    echo "Database credentials will be preserved."

    ARCHIVYN_VERSION="$(
        get_env_value ARCHIVYN_VERSION
    )"

    ARCHIVYN_PORT="$(
        get_env_value ARCHIVYN_PORT
    )"

    POSTGRES_DB="$(
        get_env_value POSTGRES_DB
    )"

    POSTGRES_USER="$(
        get_env_value POSTGRES_USER
    )"

    POSTGRES_PASSWORD="$(
        get_env_value POSTGRES_PASSWORD
    )"

    POSTGRES_DATA_PATH="$(
        get_env_value POSTGRES_DATA_PATH
    )"

    DOCUMENTS_PATH="$(
        get_env_value DOCUMENTS_PATH
    )"

    ARCHIVYN_VERSION="${ARCHIVYN_VERSION:-latest}"
    ARCHIVYN_PORT="${ARCHIVYN_PORT:-7421}"
    POSTGRES_DB="${POSTGRES_DB:-archivyn}"
    POSTGRES_USER="${POSTGRES_USER:-archivyn}"

    if [[ -z "$POSTGRES_PASSWORD" ]]; then
        fail "$ENV_FILE does not contain POSTGRES_PASSWORD."
    fi

    # Older installations will not have these settings yet.
    # Ask for them once and then save them in .env.
    if [[ -z "$POSTGRES_DATA_PATH" ]]; then
        POSTGRES_DATA_PATH="$(
            prompt_absolute_path \
                "SQL data storage path" \
                "/data"
        )"
    fi

    if [[ -z "$DOCUMENTS_PATH" ]]; then
        DOCUMENTS_PATH="$(
            prompt_absolute_path \
                "Document storage path" \
                "/documents"
        )"
    fi
else
    ARCHIVYN_VERSION="latest"

    ARCHIVYN_PORT="$(
        prompt_port "7421"
    )"

    POSTGRES_DB="$(
        prompt_database_identifier \
            "Database name" \
            "archivyn"
    )"

    POSTGRES_USER="$(
        prompt_database_identifier \
            "Database user" \
            "archivyn"
    )"

    POSTGRES_DATA_PATH="$(
        prompt_absolute_path \
            "SQL data storage path" \
            "/data"
    )"

    DOCUMENTS_PATH="$(
        prompt_absolute_path \
            "Document storage path" \
            "/documents"
    )"

    POSTGRES_PASSWORD="$(
        create_password
    )"
fi

if [[ "$POSTGRES_DATA_PATH" == "$DOCUMENTS_PATH" ]]; then
    fail \
        "the SQL data path and document path must be different."
fi

echo
echo "Creating storage directories..."

# Creates the selected host directories.
install \
    -d \
    -m 0700 \
    "$POSTGRES_DATA_PATH"

install \
    -d \
    -m 0750 \
    "$DOCUMENTS_PATH"

# Keep documents private from normal host users.
chown root:root "$DOCUMENTS_PATH"
chmod 0750 "$DOCUMENTS_PATH"

umask 077

cat > "$ENV_FILE" <<ENV
ARCHIVYN_VERSION=${ARCHIVYN_VERSION}
ARCHIVYN_PORT=${ARCHIVYN_PORT}

POSTGRES_DB=${POSTGRES_DB}
POSTGRES_USER=${POSTGRES_USER}
POSTGRES_PASSWORD=${POSTGRES_PASSWORD}

POSTGRES_DATA_PATH=${POSTGRES_DATA_PATH}
DOCUMENTS_PATH=${DOCUMENTS_PATH}
ENV

chown \
    "$INSTALL_USER:$INSTALL_GROUP" \
    "$ENV_FILE"

chmod 0600 "$ENV_FILE"

# Do not keep the password in the shell environment any longer than needed.
unset POSTGRES_PASSWORD

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

      # This is the path seen inside the Archivyn container.
      # The host location is selected during installation.
      DocumentStorage__Path: /documents

      ConnectionStrings__Archivyn: >-
        Host=postgres;
        Port=5432;
        Database=${POSTGRES_DB};
        Username=${POSTGRES_USER};
        Password=${POSTGRES_PASSWORD};

    volumes:
      - type: bind
        source: ${DOCUMENTS_PATH}
        target: /documents

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
      - type: bind
        source: ${POSTGRES_DATA_PATH}
        target: /var/lib/postgresql/data

    healthcheck:
      test:
        - CMD-SHELL
        - pg_isready -U ${POSTGRES_USER} -d ${POSTGRES_DB}
      interval: 5s
      timeout: 5s
      retries: 10
COMPOSE

chown \
    "$INSTALL_USER:$INSTALL_GROUP" \
    "$COMPOSE_FILE"

chmod 0644 "$COMPOSE_FILE"

echo
echo "Validating configuration..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    config \
    >/dev/null

echo "Pulling containers..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    pull

# Obtain the PostgreSQL user and group IDs directly from the image instead
# of assuming a particular numeric UID.
POSTGRES_UID="$(
    docker run \
        --rm \
        --entrypoint sh \
        postgres:17 \
        -c 'id -u postgres'
)"

POSTGRES_GID="$(
    docker run \
        --rm \
        --entrypoint sh \
        postgres:17 \
        -c 'id -g postgres'
)"

chown \
    -R \
    "$POSTGRES_UID:$POSTGRES_GID" \
    "$POSTGRES_DATA_PATH"

chmod 0700 "$POSTGRES_DATA_PATH"

# Migrate PostgreSQL data from the older named Docker volume when:
#
# 1. The old volume exists.
# 2. The newly selected SQL directory is empty.
if docker volume inspect "$LEGACY_DATABASE_VOLUME" \
       >/dev/null 2>&1 &&
   [[ -z "$(
       find \
           "$POSTGRES_DATA_PATH" \
           -mindepth 1 \
           -print \
           -quit
   )" ]]
then
    echo
    echo \
        "Migrating the existing PostgreSQL database to $POSTGRES_DATA_PATH..."

    # Stop the old containers before copying the database.
    docker stop \
        archivyn \
        archivyn-postgres \
        >/dev/null 2>&1 ||
        true

    docker run \
        --rm \
        --volume \
            "${LEGACY_DATABASE_VOLUME}:/source:ro" \
        --volume \
            "${POSTGRES_DATA_PATH}:/destination" \
        postgres:17 \
        bash -c \
            'cp -a /source/. /destination/'

    chown \
        -R \
        "$POSTGRES_UID:$POSTGRES_GID" \
        "$POSTGRES_DATA_PATH"

    chmod 0700 "$POSTGRES_DATA_PATH"
fi

echo
echo "Starting Archivyn..."

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    up \
    -d \
    --remove-orphans

echo "Checking Archivyn..."

sleep 8

if [[ "$(
    docker inspect \
        -f '{{.State.Running}}' \
        archivyn \
        2>/dev/null ||
        true
)" != "true" ]]
then
    echo
    echo "Archivyn failed to start:"

    docker logs \
        --tail=100 \
        archivyn ||
        true

    exit 1
fi

docker compose \
    --env-file "$ENV_FILE" \
    -f "$COMPOSE_FILE" \
    ps

SERVER_IP="$(
    hostname -I |
        awk '{print $1}'
)"

echo
echo "Archivyn installation complete."
echo "Open: http://${SERVER_IP}:${ARCHIVYN_PORT}"
echo
echo "Configuration files: $INSTALL_DIR"
echo "SQL data: $POSTGRES_DATA_PATH"
echo "Documents: $DOCUMENTS_PATH"