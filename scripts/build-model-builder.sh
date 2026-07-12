#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "$0")/.." && pwd)"
rid="${1:-}"
if [[ -z "$rid" ]]; then
  machine="$(uname -m)"
  [[ "$machine" == "x86_64" ]] && architecture="x64" || architecture="arm64"
  [[ "$(uname -s)" == "Darwin" ]] && platform="osx" || platform="linux"
  rid="$platform-$architecture"
fi

# Use the exact Nintendo.Bfres build shipped by the official Milk Bar 2.0.1
# launcher. Its Wii U writer differs from the public BfresLibrary NuGet package.
official_dir="$root/Build/dependencies/milkbar-official"
official_bfres="$official_dir/BfresLibrary.dll"
if [[ ! -f "$official_bfres" ]]; then
  mkdir -p "$official_dir"
  official_zip="$official_dir/MilkBarLauncher-2.0.1.zip"
  if [[ ! -f "$official_zip" ]]; then
    curl -fL "https://github.com/MilkBarModding/MilkBarLauncher/releases/download/2.0.1/MilkBarLauncher.zip" -o "$official_zip"
  fi
  for dependency in \
    BfresLibrary.dll \
    Syroot.BinaryData.Core.dll \
    Syroot.BinaryData.dll \
    Syroot.Maths.dll \
    Syroot.NintenTools.NSW.Bntx.dll; do
    unzip -jo "$official_zip" "$dependency" -d "$official_dir" >/dev/null
  done
fi

dotnet="${DOTNET:-$(command -v dotnet || true)}"
if [[ -z "$dotnet" ]]; then
  dotnet="$root/.tools/dotnet/dotnet"
  if [[ ! -x "$dotnet" ]]; then
    mkdir -p "$root/.tools/dotnet"
    installer="${TMPDIR:-/tmp}/milkbar-dotnet-install.sh"
    curl -fsSL https://dot.net/v1/dotnet-install.sh -o "$installer"
    bash "$installer" --channel 8.0 --install-dir "$root/.tools/dotnet"
  fi
fi

output="$root/Build/model-builder/$rid"
"$dotnet" publish "$root/CrossPlatform/ModelBuilder/MilkBar.ModelBuilder.csproj" \
  -c Release -r "$rid" --self-contained true \
  -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true \
  -p:DebugType=None -p:DebugSymbols=false -o "$output"

echo "Model builder built at $output/milkbar-model-builder"
