#!/usr/bin/env bash
set -euo pipefail
root="$(cd "$(dirname "$0")/.." && pwd)"
python="$root/.venv/bin/python"
if [[ ! -x "$python" ]]; then
  python="python3"
fi
exec "$python" "$root/CrossPlatform/milkbar_qt_gui.py"
