#!/usr/bin/env python3
"""Make direct remote-actor refreshes bypass the early deleteLater HLE hook."""

from pathlib import Path
import sys


def main() -> int:
    if len(sys.argv) != 2:
        print(f"Usage: {Path(sys.argv[0]).name} patch_UKL_ActorInterceptor.asm", file=sys.stderr)
        return 2

    path = Path(sys.argv[1])
    text = path.read_text(encoding="utf-8")
    marker = "; MILKBAR_DIRECT_DELETE_BYPASS"
    if marker in text:
        return 0

    anchor = "ActorDeleteLaterHook:\n; Backup LR"
    replacement = (
        "ActorDeleteLaterHook:\n"
        f"{marker}\n"
        "; Delete reason 0x7f is reserved for the native equipment refresh.\n"
        "; Calling this hooked entry through the shared function dispatcher must\n"
        "; not recursively enter an HLE callback before deleteLater returns. The\n"
        "; real ActorErase hook reports completion after BOTW releases the actor.\n"
        "cmpwi r4, 0x7f\n"
        "bne ActorDeleteLaterNotify\n"
        "li r4, 0\n"
        "mflr r0\n"
        "b 0x0378a378\n\n"
        "ActorDeleteLaterNotify:\n"
        "; Backup LR"
    )
    count = text.count(anchor)
    if count != 1:
        raise RuntimeError(f"Expected one actor delete hook anchor, found {count}")
    path.write_text(text.replace(anchor, replacement, 1), encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
