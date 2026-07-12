#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "$0")/.." && pwd)"
build="$root/Build/cross-platform"

cmake_args=(-S "$root/DLL/InjectDLL" -B "$build" -DCMAKE_BUILD_TYPE=Release)
if [[ "$(uname -s)" == "Darwin" ]]; then
  # Cemu 2.6 for macOS is x86-64. Current development builds can be arm64,
  # so produce a universal client that works with either executable.
  cmake_args+=("-DCMAKE_OSX_ARCHITECTURES=x86_64;arm64")
fi

cmake "${cmake_args[@]}"
cmake --build "$build" --config Release --parallel
echo "Built native Milk Bar client in $build"
