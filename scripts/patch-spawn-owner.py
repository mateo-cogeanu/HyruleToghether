#!/usr/bin/env python3
"""Make the bundled BOTW actor-spawn handshake safe across PPC cores."""

from pathlib import Path
import sys


def replace_once(text: str, old: str, new: str, description: str) -> str:
    count = text.count(old)
    if count != 1:
        raise RuntimeError(f"Expected one {description} anchor, found {count}")
    return text.replace(old, new, 1)


def main() -> int:
    if len(sys.argv) != 2:
        print(f"Usage: {Path(sys.argv[0]).name} patch_SpawnActors.asm", file=sys.stderr)
        return 2

    path = Path(sys.argv[1])
    text = path.read_text(encoding="utf-8")
    marker = "; MILKBAR_CROSS_CORE_SPAWN_OWNER"
    if marker in text:
        text = text.replace("SpawnOwnerStack", "SpawnOwnerToken")
        text = text.replace("cmpw r1, r11", "cmpw r12, r11")
        text = text.replace(
            "belong only to the invocation whose post-backup stack pointer matches.",
            "belong only to the invocation carrying the matching saved r12 token.",
        )
        text = text.replace(
            "Stack pointer of the CallFunction invocation that owns the staged GPRs.",
            "Explicit r12 token of the CallFunction invocation that owns the staged GPRs.",
        )
        path.write_text(text, encoding="utf-8")
        return 0

    text = replace_once(
        text,
        "InterceptRegisters:\n.byte 0\n\nFunctionToJump:",
        "InterceptRegisters:\n.byte 0\n\n"
        f"{marker}\n"
        "; Explicit r12 token of the CallFunction invocation that owns the staged GPRs.\n"
        "SpawnOwnerToken:\n.int 0\n\n"
        "FunctionToJump:",
        "spawn-transfer layout",
    )
    text = replace_once(
        text,
        "beq restoreAndExit ; We can skip over applying params if we're not calling the func, so putting this here is fine.\n\n"
        "; Set up where to jump to...",
        "beq restoreAndExit ; We can skip over applying params if we're not calling the func, so putting this here is fine.\n\n"
        "; HLE callbacks may overlap on Cemu's three PPC cores. The staged GPRs\n"
        "; belong only to the invocation carrying the matching saved r12 token.\n"
        "lis r11, SpawnOwnerToken@ha\n"
        "lwz r11, SpawnOwnerToken@l(r11)\n"
        "cmpw r12, r11\n"
        "bne restoreAndExit\n\n"
        "; Set up where to jump to...",
        "spawn request consumer",
    )
    path.write_text(text, encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
