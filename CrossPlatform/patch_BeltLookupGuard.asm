[BotW_MilkBar_BeltLookupGuard_V208]
moduleMatches = 0x6267BFD0

; Player-derived multiplayer actors do not always expose Link's Mt_Belt_A and
; Mt_Belt_C model entries. BOTW's belt visibility helper dereferences the
; failed lookup without checking it. The helper is purely cosmetic, so skip it
; to prevent both worker-thread and inventory-triggered null dereferences.
0x02b07658 = blr
