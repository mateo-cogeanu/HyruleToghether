import tempfile
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path

import milkbar_launcher as launcher


class CemuGraphicPackSettingsTests(unittest.TestCase):
    def test_configure_enables_required_packs_once(self) -> None:
        with tempfile.TemporaryDirectory() as temp:
            config = Path(temp)
            merged = config / "graphicPacks/BreathOfTheWild_UKMM/rules.txt"
            extended = config / "graphicPacks/downloadedGraphicPacks/BreathOfTheWild/Mods/ExtendedMemory/rules.txt"
            merged.parent.mkdir(parents=True)
            extended.parent.mkdir(parents=True)
            merged.touch()
            extended.touch()

            launcher._configure_cemu_settings(config)
            launcher._configure_cemu_settings(config)

            entries = [
                entry.get("filename")
                for entry in ET.parse(config / "settings.xml").getroot().findall("./GraphicPack/Entry")
            ]
            self.assertEqual(entries.count("graphicPacks/BreathOfTheWild_UKMM/rules.txt"), 1)
            self.assertEqual(
                entries.count(
                    "graphicPacks/downloadedGraphicPacks/BreathOfTheWild/Mods/ExtendedMemory/rules.txt"
                ),
                1,
            )

    def test_missing_extended_memory_pack_is_rejected(self) -> None:
        with tempfile.TemporaryDirectory() as temp:
            with self.assertRaisesRegex(RuntimeError, "Extended Memory"):
                launcher._require_extended_memory_pack(Path(temp))


if __name__ == "__main__":
    unittest.main()
