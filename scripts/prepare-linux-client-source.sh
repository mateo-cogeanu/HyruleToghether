#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "$0")/.." && pwd)"
source_dir="$root/DLL/InjectDLL"
output_dir="$root/Build/linux-client-source"

rm -rf "$output_dir"
mkdir -p "$output_dir"
cp -R "$source_dir/." "$output_dir/"

# The original Windows code uses MSVC extensions accepted as warnings by Apple
# Clang but rejected by GCC. Normalize only the generated Linux copy, leaving
# the tested macOS and Windows sources byte-for-byte unchanged.
find "$output_dir" -type f \( -name '*.h' -o -name '*.cpp' \) -exec \
  sed -i -E \
    -e 's/([[:space:]])static class /\1class /g' \
    -e 's/^([[:space:]]*)extern (struct|union) /\1\2 /g' {} +

if ! grep -q '^#include <cmath>$' "$output_dir/InterpolationFunctions.cpp"; then
  sed -i '1i#include <cmath>' "$output_dir/InterpolationFunctions.cpp"
fi

echo "Prepared GCC-compatible Linux client sources at $output_dir"
