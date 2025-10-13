#!/usr/bin/env bash
set -euo pipefail

# ===== Configuration =====
RULESET_URL="https://github.com/fuyukiSmkw/osu-chat-notifier/releases/latest/download/osu.Game.Rulesets.ChatNotifier.dll"
RULESET_NAME="osu.Game.Rulesets.ChatNotifier.dll"

echo
echo "============================================"
echo " osu!lazer Chat Notifier Installer"
echo "============================================"
echo

# ===== Detect osu!lazer installation path =====
# Default paths
LOCAL_PATH="${HOME}/.local/share/osu"
FLATPAK_PATH="${HOME}/.var/app/sh.ppy.osu/data/osu"

if [ -d "$LOCAL_PATH" ]; then
    OSU_DATA_PATH="$LOCAL_PATH"
    echo "Found osu!lazer folder at: $OSU_DATA_PATH"
elif [ -d "$FLATPAK_PATH" ]; then
    OSU_DATA_PATH="$FLATPAK_PATH"
    echo "Found osu!lazer Flatpak folder at: $OSU_DATA_PATH"
else
    echo "[Error] Could not find osu!lazer data folder from expected locations:"
    echo "  - $LOCAL_PATH"
    echo "  - $FLATPAK_PATH"
    echo
    echo "Install osu!lazer first."
    exit 1
fi

# Try to read custom data path from storage.ini
INI_FILE="${OSU_DATA_PATH}/storage.ini"
if [ -f "$INI_FILE" ]; then
    # Read line FullPath
    INI_PATH="$(grep -E '^FullPath[[:space:]]*=' "$INI_FILE" | sed -E 's/^FullPath[[:space:]]*=[[:space:]]*//')"

    # Remove spaces
    INI_PATH="$(echo "$INI_PATH" | sed 's/^[[:space:]]*//; s/[[:space:]]*$//')"

    if [ -n "$INI_PATH" ] && [ -d "$INI_PATH" ]; then
        echo "Found migrated osu! data path from storage.ini: $INI_PATH"
        OSU_DATA_PATH="$INI_PATH"
    else
        echo "storage.ini found, but FullPath is empty or invalid. Using default osu! data path."
    fi
fi

RULESET_DIR="${OSU_DATA_PATH}/rulesets"

# ===== Check or create rulesets folder =====
if [ ! -d "$RULESET_DIR" ]; then
    echo "The rulesets folder does not exist. Creating it now..."
    mkdir -p "$RULESET_DIR" || {
        echo "[Error] Failed to create folder: $RULESET_DIR"
        echo "Try running this script with sudo or check folder permissions."
        exit 1
    }
fi
echo "Using rulesets folder: $RULESET_DIR"

# ===== Download the DLL =====
echo
echo "Downloading ${RULESET_NAME} to ${RULESET_DIR}..."

if command -v curl >/dev/null 2>&1; then
    DL_CMD="curl -L --fail -o \"${RULESET_DIR}/${RULESET_NAME}\" \"${RULESET_URL}\""
elif command -v wget >/dev/null 2>&1; then
    DL_CMD="wget -O \"${RULESET_DIR}/${RULESET_NAME}\" \"${RULESET_URL}\""
else
    echo "[Error] Neither curl nor wget is installed."
    exit 1
fi

if ! eval "$DL_CMD"; then
    echo "[Error] Download failed. Please check your internet connection."
    exit 1
fi

# ===== Success =====
echo
echo "Installation complete!"
echo "Installed to: ${RULESET_DIR}/${RULESET_NAME}"
echo
exit 0
