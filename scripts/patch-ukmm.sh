#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 ]]; then
  echo "Usage: $0 /path/to/UKMM-source" >&2
  exit 2
fi

source_root="$(cd "$1" && pwd)"
cli="$source_root/src/cli.rs"
packer="$source_root/crates/uk-mod/src/pack.rs"
[[ -f "$cli" && -f "$packer" ]] || { echo "Not a UKMM source tree: $source_root" >&2; exit 1; }

python3 - "$cli" "$packer" <<'PY'
from pathlib import Path
import sys

path = Path(sys.argv[1])
packer_path = Path(sys.argv[2])
text = path.read_text(encoding="utf-8")
marker = "MILKBAR_BNP_CLI"
if marker not in text:
    needle = "    fn check_mod(&self, path: &Path) -> Result<Option<PathBuf>> {\n"
    if needle not in text:
        raise SystemExit("UKMM's CLI changed; update scripts/patch-ukmm.sh")
    block = '''    fn check_mod(&self, path: &Path) -> Result<Option<PathBuf>> {
        // MILKBAR_BNP_CLI: the GUI supports BNP conversion, but upstream's
        // CLI currently routes BNP files through the generic archive reader.
        if path
            .extension()
            .and_then(|extension| extension.to_str())
            .is_some_and(|extension| extension.eq_ignore_ascii_case("bnp"))
        {
            let converted = uk_manager::bnp::convert_bnp(&self.core, path)
                .context("Failed to convert BNP to UKMM mod")?;
            let mod_ = ModReader::open(&converted, vec![])
                .context("Failed to open converted BNP")?;
            if !mod_.meta.options.is_empty() {
                anyhow_ext::bail!(
                    "This mod contains configuration options and should be installed via the GUI."
                );
            }
            println!("Installing {}...", mod_.meta.name);
            return Ok(Some(converted));
        }
'''
    path.write_text(text.replace(needle, block), encoding="utf-8")

text = path.read_text(encoding="utf-8")
if "MILKBAR_UKMM_CONFIG" not in text:
    text = text.replace(
        "    path::{Path, PathBuf},\n",
        "    path::{Path, PathBuf},\n    sync::Arc,\n",
    )
    text = text.replace(
        "use uk_manager::{core, mods::LookupMod, settings::Platform};",
        """use uk_content::{constants::Language, prelude::Endian};
use uk_manager::{
    core,
    mods::LookupMod,
    settings::{DeployConfig, DeployLayout, DeployMethod, Platform, PlatformSettings, Settings},
};""",
    )
    text = text.replace(
        "use uk_mod::{unpack::ModReader, Manifest, Meta};",
        "use uk_mod::{unpack::ModReader, Manifest, Meta};\nuse uk_reader::ResourceReader;",
    )
    needle = """impl Runner {
    pub fn new(cli: Ukmm) -> Self {
"""
    replacement = """impl Runner {
    pub fn new(cli: Ukmm) -> Self {
        // MILKBAR_UKMM_CONFIG: configure the private unattended merge from
        // launcher-provided paths before UKMM initializes its managers.
        if let (Ok(base), Ok(update), Ok(output), Ok(storage)) = (
            std::env::var("MILKBAR_UKMM_BASE"),
            std::env::var("MILKBAR_UKMM_UPDATE"),
            std::env::var("MILKBAR_UKMM_OUTPUT"),
            std::env::var("MILKBAR_UKMM_STORAGE"),
        ) {
            let aoc = std::env::var("MILKBAR_UKMM_AOC").ok().filter(|path| !path.is_empty());
            let language = std::env::var("MILKBAR_UKMM_LANGUAGE")
                .ok().and_then(|language| language.parse().ok()).unwrap_or(Language::USen);
            let dump = ResourceReader::from_unpacked_dirs(
                Some(base), Some(update), aoc, Endian::Big,
            ).expect("Milk Bar provided invalid BOTW dump folders");
            let mut settings = Settings::default();
            settings.current_mode = Platform::WiiU;
            settings.system_7z = false;
            settings.storage_dir = storage.into();
            settings.check_updates = uk_manager::settings::UpdatePreference::None;
            settings.wiiu_config = Some(PlatformSettings {
                language,
                profile: "Default".into(),
                dump: Arc::new(dump),
                deploy_config: Some(DeployConfig {
                    output: output.into(),
                    method: DeployMethod::Copy,
                    auto: true,
                    cemu_rules: true,
                    executable: None,
                    layout: DeployLayout::WithName,
                }),
            });
            settings.save().expect("Failed to save Milk Bar UKMM settings");
        }
"""
    if needle not in text:
        raise SystemExit("UKMM's Runner changed; update scripts/patch-ukmm.sh")
    path.write_text(text.replace(needle, replacement), encoding="utf-8")

packer = packer_path.read_text(encoding="utf-8")
if "MILKBAR_OPAQUE_NEW_SARC" not in packer:
    needle = """                let resource = ResourceData::from_binary(name.as_str(), &*file_data)
                    .with_context(|| jstr!("Failed to parse resource {&name}"))?;
"""
    replacement = """                // MILKBAR_OPAQUE_NEW_SARC: custom actor packs are new files,
                // so preserve them whole instead of diffing their non-stock resources.
                let resource = if self.hash_table.is_file_new(&canon)
                    && is_mergeable_sarc(canon.as_str(), file_data.as_ref())
                {
                    ResourceData::Binary(file_data.to_vec())
                } else {
                    ResourceData::from_binary(name.as_str(), &*file_data)
                        .with_context(|| jstr!("Failed to parse resource {&name}"))?
                };
"""
    if needle not in packer:
        raise SystemExit("UKMM's resource packer changed; update scripts/patch-ukmm.sh")
    packer = packer.replace(needle, replacement)
    condition = "if !is_mergeable && is_mergeable_sarc(canon.as_str(), file_data.as_ref()) {"
    guarded = """if !is_mergeable
                    && is_mergeable_sarc(canon.as_str(), file_data.as_ref())
                    && !self.hash_table.is_file_new(&canon)
                {"""
    if condition not in packer:
        raise SystemExit("UKMM's nested SARC condition changed; update scripts/patch-ukmm.sh")
    packer_path.write_text(packer.replace(condition, guarded), encoding="utf-8")
PY

echo "Patched UKMM CLI for unattended Milk Bar BNP conversion."
