#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 ]]; then
  echo "Usage: $0 /path/to/Cemu-source" >&2
  exit 2
fi

source_root="$(cd "$1" && pwd)"
header="$source_root/src/Common/precompiled.h"
cmake_file="$source_root/src/CMakeLists.txt"
app_source="$source_root/src/gui/wxgui/CemuApp.cpp"
os_common_header="$source_root/src/Cafe/OS/common/OSCommon.h"
os_common_source="$source_root/src/Cafe/OS/common/OSCommon.cpp"
dynload_source="$source_root/src/Cafe/OS/libs/coreinit/coreinit_DynLoad.cpp"
msl_header="$source_root/src/Cafe/HW/Latte/LegacyShaderDecompiler/LatteDecompilerEmitMSLHeader.hpp"
rpl_source="$source_root/src/Cafe/OS/RPL/rpl.cpp"
launch_header="$source_root/src/config/LaunchSettings.h"
launch_source="$source_root/src/config/LaunchSettings.cpp"

if [[ ! -f "$header" || ! -f "$cmake_file" ]]; then
  echo "Not a current cemu-project/Cemu source tree: $source_root" >&2
  exit 1
fi

python3 - "$header" "$cmake_file" "$app_source" "$os_common_header" "$os_common_source" "$dynload_source" "$launch_header" "$launch_source" "$msl_header" "$rpl_source" <<'PY'
from pathlib import Path
import sys

header = Path(sys.argv[1])
cmake_file = Path(sys.argv[2])
app_source = Path(sys.argv[3])
os_common_header = Path(sys.argv[4])
os_common_source = Path(sys.argv[5])
dynload_source = Path(sys.argv[6])
launch_header = Path(sys.argv[7])
launch_source = Path(sys.argv[8])
msl_header = Path(sys.argv[9])
rpl_source = Path(sys.argv[10])
text = header.read_text()
old = """    #if BOOST_OS_WINDOWS
        #define DLLEXPORT __attribute__((dllexport))
    #else
        #define DLLEXPORT
    #endif"""
new = """    #if BOOST_OS_WINDOWS
        #define DLLEXPORT __attribute__((dllexport))
    #else
        // Milk Bar loads as a native shared library and resolves Cemu's public
        // emulator hooks at runtime.
        #define DLLEXPORT __attribute__((visibility(\"default\")))
    #endif"""
if old in text:
    header.write_text(text.replace(old, new))
elif "visibility(\"default\")" not in text:
    raise SystemExit("Cemu's DLLEXPORT block changed; update scripts/patch-cemu.sh")

cmake = cmake_file.read_text()
marker = "add_executable(CemuBin"
if "ENABLE_EXPORTS ON" not in cmake:
    position = cmake.find(marker)
    if position < 0:
        raise SystemExit("Cemu's CMake target changed; update scripts/patch-cemu.sh")
    # ENABLE_EXPORTS causes symbols marked above to enter the executable's
    # dynamic symbol table on ELF and Mach-O.
    insertion = "set_target_properties(CemuBin PROPERTIES ENABLE_EXPORTS ON)\n\n"
    # The target must exist before setting its properties. Insert after the
    # closing parenthesis of the add_executable source list.
    closing = cmake.find("\n)", position)
    cmake = cmake[:closing + 3] + insertion + cmake[closing + 3:]
    cmake_file.write_text(cmake)

# Static-library dead stripping can discard these entry points even when their
# source visibility is public. Retain and export them explicitly in the final
# Mach-O/ELF executable.
cmake = cmake_file.read_text()
link_marker = "MILKBAR_EXPORTED_HOOKS"
if link_marker not in cmake:
    block = """

# MILKBAR_EXPORTED_HOOKS
if(APPLE)
    target_link_options(CemuBin PRIVATE
        "-Wl,-exported_symbol,_memory_getBase"
        "-Wl,-exported_symbol,_osLib_registerHLEFunction"
        "-Wl,-exported_symbol,_milkbar_isHLEReady"
        "-Wl,-exported_symbol,_milkbar_markHooksReady")
elseif(UNIX)
    target_link_options(CemuBin PRIVATE
        "-Wl,--export-dynamic-symbol=memory_getBase"
        "-Wl,--export-dynamic-symbol=osLib_registerHLEFunction"
        "-Wl,--export-dynamic-symbol=milkbar_isHLEReady"
        "-Wl,--export-dynamic-symbol=milkbar_markHooksReady")
endif()
"""
    cmake_file.write_text(cmake + block)

cmake = cmake_file.read_text()
if "milkbar_isHLEReady" not in cmake:
    cmake = cmake.replace(
        '"-Wl,-exported_symbol,_osLib_registerHLEFunction")',
        '"-Wl,-exported_symbol,_osLib_registerHLEFunction"\n'
        '        "-Wl,-exported_symbol,_milkbar_isHLEReady")')
    cmake = cmake.replace(
        '"-Wl,--export-dynamic-symbol=osLib_registerHLEFunction")',
        '"-Wl,--export-dynamic-symbol=osLib_registerHLEFunction"\n'
        '        "-Wl,--export-dynamic-symbol=milkbar_isHLEReady")')
    cmake_file.write_text(cmake)
cmake = cmake_file.read_text()
if "milkbar_markHooksReady" not in cmake:
    cmake = cmake.replace(
        '"-Wl,-exported_symbol,_milkbar_isHLEReady")',
        '"-Wl,-exported_symbol,_milkbar_isHLEReady"\n'
        '        "-Wl,-exported_symbol,_milkbar_markHooksReady")')
    cmake = cmake.replace(
        '"-Wl,--export-dynamic-symbol=milkbar_isHLEReady")',
        '"-Wl,--export-dynamic-symbol=milkbar_isHLEReady"\n'
        '        "-Wl,--export-dynamic-symbol=milkbar_markHooksReady")')
    cmake_file.write_text(cmake)

# Give the bundled launcher a private writable Cemu configuration directory.
# This suppresses Cemu's first-run setup without changing another Cemu install.
app = app_source.read_text()
settings_call = "\tActiveSettings::SetPaths(isPortable, exePath, user_data_path, config_path, cache_path, data_path, failedWriteAccess);"
override = """\t// MILKBAR_PRIVATE_CEMU_CONFIG
\tif (const char* milkbarPath = std::getenv("MILKBAR_CEMU_DATA_DIR"); milkbarPath && *milkbarPath)
\t\tuser_data_path = config_path = cache_path = fs::path(milkbarPath);
\tActiveSettings::SetPaths(isPortable, exePath, user_data_path, config_path, cache_path, data_path, failedWriteAccess);"""
if "MILKBAR_PRIVATE_CEMU_CONFIG" not in app:
    if settings_call not in app:
        raise SystemExit("Cemu's path setup changed; update scripts/patch-cemu.sh")
    app_source.write_text(app.replace(settings_call, override))

# Open Cemu's own graphic-pack manager directly from the Milk Bar settings
# page, filtered to BOTW, without launching the game.
launch_h = launch_header.read_text()
if "GetGraphicPacksTitleID" not in launch_h:
    launch_h = launch_h.replace(
        "static std::optional<uint64> GetLoadTitleID() {return s_load_title_id;}",
        "static std::optional<uint64> GetLoadTitleID() {return s_load_title_id;}\n"
        "\tstatic std::optional<uint64> GetGraphicPacksTitleID() { return s_graphic_packs_title_id; }")
    launch_h = launch_h.replace(
        "inline static std::optional<uint64> s_load_title_id{};",
        "inline static std::optional<uint64> s_load_title_id{};\n"
        "\tinline static std::optional<uint64> s_graphic_packs_title_id{};")
    launch_header.write_text(launch_h)

launch_cpp = launch_source.read_text()
if '("graphic-packs"' not in launch_cpp:
    launch_cpp = launch_cpp.replace(
        '("title-id,t", po::value<std::string>(), "Title ID of the title to be launched (overridden by --game)")',
        '("title-id,t", po::value<std::string>(), "Title ID of the title to be launched (overridden by --game)")\n'
        '\t\t("graphic-packs", po::value<std::string>(), "Open the graphic pack manager filtered to a title ID")')
    anchor = '\t\tif (vm.count("mlc"))'
    block = '''\t\tif (vm.count("graphic-packs"))
\t\t{
\t\t\tauto value = vm["graphic-packs"].as<std::string>();
\t\t\tif (value.starts_with('='))
\t\t\t\tvalue.erase(value.begin());
\t\t\ttry { s_graphic_packs_title_id = std::stoull(value, nullptr, 16); }
\t\t\tcatch (const std::invalid_argument&) { std::cerr << "Expected a hexadecimal title ID for --graphic-packs\\n"; }
\t\t}

'''
    if anchor not in launch_cpp:
        raise SystemExit("Cemu's command-line parser changed; update scripts/patch-cemu.sh")
    launch_source.write_text(launch_cpp.replace(anchor, block + anchor))

app = app_source.read_text()
if '"wxgui/GraphicPacksWindow2.h"' not in app:
    app = app.replace('#include "wxgui/MainWindow.h"', '#include "wxgui/MainWindow.h"\n#include "wxgui/GraphicPacksWindow2.h"')
if "GetGraphicPacksTitleID" not in app:
    show = "\tm_mainFrame->Show();"
    open_manager = show + '''
\tif (auto titleId = LaunchSettings::GetGraphicPacksTitleID())
\t{
\t\tm_mainFrame->CallAfter([this, titleId]()
\t\t{
\t\t\tauto* window = new GraphicPacksWindow2(m_mainFrame, *titleId);
\t\t\twindow->Bind(wxEVT_CLOSE_WINDOW, [window](wxCloseEvent&) { window->Destroy(); });
\t\t\twindow->Show(true);
\t\t});
\t}
'''
    if show not in app:
        raise SystemExit("Cemu's main window startup changed; update scripts/patch-cemu.sh")
    app = app.replace(show, open_manager)
app_source.write_text(app)

# Metal exposes 31 texture slots but only 16 sampler slots. Deduplicate sampler
# declarations and make every generated reference use the same physical slot.
msl = msl_header.read_text()
old_sampler = 'src->addFmt(", sampler samplr{} [[sampler({})]]", i, binding);'
new_sampler = '''uint32 samplerBinding = binding % 16;
\t\t\tif (!samplerBindingUsed[samplerBinding])
\t\t\t{
\t\t\t\tsrc->addFmt(", sampler samplr{} [[sampler({})]]", samplerBinding, samplerBinding);
\t\t\t\tsamplerBindingUsed[samplerBinding] = true;
\t\t\t}'''
if 'bool samplerBindingUsed[16]' not in msl:
    msl = msl.replace('bool renderTargetIndexUsed[LATTE_NUM_COLOR_TARGET] = {false};',
                      'bool renderTargetIndexUsed[LATTE_NUM_COLOR_TARGET] = {false};\n\t    bool samplerBindingUsed[16] = {false};')
if old_sampler in msl:
    msl = msl.replace(old_sampler, new_sampler)
elif new_sampler not in msl:
    raise SystemExit("Cemu's Metal sampler mapping changed; update scripts/patch-cemu.sh")
msl_header.write_text(msl)

msl_cpp = root / "src/Cafe/HW/Latte/LegacyShaderDecompiler/LatteDecompilerEmitMSL.cpp"
cpp = msl_cpp.read_text()
cpp = cpp.replace('src->addFmt("(samplr{}, ", texInstruction->textureFetch.textureIndex);',
                  'src->addFmt("(samplr{}, ", shaderContext->output->resourceMappingMTL.textureUnitToBindingPoint[texInstruction->textureFetch.textureIndex] % 16);')
cpp = cpp.replace('src->addFmt(", samplr{}, ", texInstruction->textureFetch.textureIndex);',
                  'src->addFmt(", samplr{}, ", shaderContext->output->resourceMappingMTL.textureUnitToBindingPoint[texInstruction->textureFetch.textureIndex] % 16);')
for dimension in ('float3', 'float2'):
    cpp = cpp.replace('texInstruction->textureFetch.textureIndex, texInstruction->textureFetch.textureIndex, _getRegisterVarName',
                      'texInstruction->textureFetch.textureIndex, shaderContext->output->resourceMappingMTL.textureUnitToBindingPoint[texInstruction->textureFetch.textureIndex] % 16, _getRegisterVarName')
msl_cpp.write_text(cpp)

# The native client is preloaded before Cemu finishes registering coreinit's
# HLE exports. Expose an explicit readiness signal so external callbacks are
# registered only after InitializeDynLoad has completed.
common_header = os_common_header.read_text()
ready_declaration = "void milkbar_markHLEReady();"
if ready_declaration not in common_header:
    common_header = common_header.replace(
        "void osLib_returnFromFunction(PPCInterpreter_t* hCPU, uint32 returnValue);",
        ready_declaration + "\nvoid osLib_returnFromFunction(PPCInterpreter_t* hCPU, uint32 returnValue);")
    os_common_header.write_text(common_header)

common_header = os_common_header.read_text()
if "bool osLib_hasLibrary(const char* libraryName);" not in common_header:
    common_header = common_header.replace(
        "sint32 osLib_getFunctionIndex(const char* libraryName, const char* functionName);",
        "sint32 osLib_getFunctionIndex(const char* libraryName, const char* functionName);\n"
        "bool osLib_hasLibrary(const char* libraryName);")
    os_common_header.write_text(common_header)

common_header = os_common_header.read_text()
hooks_declaration = "void milkbar_waitForHooks();"
if hooks_declaration not in common_header:
    common_header = common_header.replace(ready_declaration, ready_declaration + "\n" + hooks_declaration)
    os_common_header.write_text(common_header)

common_source = os_common_source.read_text()
ready_marker = "MILKBAR_HLE_READY"
if ready_marker not in common_source:
    insertion = """

// MILKBAR_HLE_READY
static std::atomic_bool s_milkbarHLEReady{false};
static std::atomic_bool s_milkbarHooksReady{false};
void milkbar_markHLEReady()
{
    s_milkbarHLEReady.store(true, std::memory_order_release);
}
extern "C" DLLEXPORT bool milkbar_isHLEReady()
{
    return s_milkbarHLEReady.load(std::memory_order_acquire);
}
extern "C" DLLEXPORT void milkbar_markHooksReady()
{
    s_milkbarHooksReady.store(true, std::memory_order_release);
}
void milkbar_waitForHooks()
{
    if (const char* enabled = std::getenv("MILKBAR_WAIT_FOR_HOOKS"); !enabled || !*enabled)
        return;
    for (int attempt = 0; attempt < 10000 && !s_milkbarHooksReady.load(std::memory_order_acquire); ++attempt)
        std::this_thread::sleep_for(std::chrono::milliseconds(1));
}
"""
    anchor = "std::vector<osPointerEntry_t> osDataTable;"
    if anchor not in common_source:
        raise SystemExit("Cemu's OS function table changed; update scripts/patch-cemu.sh")
    os_common_source.write_text(common_source.replace(anchor, anchor + insertion))
common_source = os_common_source.read_text()
if "milkbar_markHooksReady" not in common_source:
    common_source = common_source.replace(
        "static std::atomic_bool s_milkbarHLEReady{false};",
        "static std::atomic_bool s_milkbarHLEReady{false};\nstatic std::atomic_bool s_milkbarHooksReady{false};")
    anchor = "extern \"C\" DLLEXPORT bool milkbar_isHLEReady()\n{\n    return s_milkbarHLEReady.load(std::memory_order_acquire);\n}"
    addition = anchor + """
extern "C" DLLEXPORT void milkbar_markHooksReady()
{
    s_milkbarHooksReady.store(true, std::memory_order_release);
}
void milkbar_waitForHooks()
{
    if (const char* enabled = std::getenv("MILKBAR_WAIT_FOR_HOOKS"); !enabled || !*enabled)
        return;
    for (int attempt = 0; attempt < 10000 && !s_milkbarHooksReady.load(std::memory_order_acquire); ++attempt)
        std::this_thread::sleep_for(std::chrono::milliseconds(1));
}
"""
    if anchor not in common_source:
        raise SystemExit("Cemu's Milk Bar readiness hook changed; update scripts/patch-cemu.sh")
    os_common_source.write_text(common_source.replace(anchor, addition))

common_source = os_common_source.read_text()
if "bool osLib_hasLibrary(const char* libraryName)" not in common_source:
    anchor = "sint32 osLib_getFunctionIndex(const char* libraryName, const char* functionName)"
    function = '''bool osLib_hasLibrary(const char* libraryName)
{
    if (!s_osFunctionTable)
        return false;
    uint32 libHashA, libHashB;
    osLib_generateHashFromName(libraryName, &libHashA, &libHashB);
    return std::ranges::any_of(*s_osFunctionTable, [&](const osFunctionEntry_t& entry) {
        return entry.libHashA == libHashA && entry.libHashB == libHashB;
    });
}

'''
    if anchor not in common_source:
        raise SystemExit("Cemu's HLE lookup changed; update scripts/patch-cemu.sh")
    os_common_source.write_text(common_source.replace(anchor, function + anchor))

rpl = rpl_source.read_text()
old_reject = "if (!dep->isCafeOSModule && !dep->rplLoaderContext)\n\t\t\t\treturn RPL_INVALID_HANDLE; // module not found"
new_reject = "if (!dep->isCafeOSModule && !dep->rplLoaderContext && !osLib_hasLibrary(moduleName.c_str()))\n\t\t\t\treturn RPL_INVALID_HANDLE; // module not found"
if old_reject in rpl:
    rpl_source.write_text(rpl.replace(old_reject, new_reject))
elif new_reject not in rpl:
    raise SystemExit("Cemu's module handle lookup changed; update scripts/patch-cemu.sh")

dynload = dynload_source.read_text()
if "milkbar_markHLEReady();" not in dynload:
    anchor = 'cafeExportRegister("coreinit", OSDynLoad_FindExport, LogType::Placeholder);'
    if anchor not in dynload:
        raise SystemExit("Cemu's DynLoad initialization changed; update scripts/patch-cemu.sh")
    dynload_source.write_text(dynload.replace(anchor, anchor + "\n\t\tmilkbar_markHLEReady();"))
dynload = dynload_source.read_text()
if "milkbar_waitForHooks();" not in dynload:
    dynload_source.write_text(dynload.replace("milkbar_markHLEReady();", "milkbar_markHLEReady();\n\t\tmilkbar_waitForHooks();"))
PY

echo "Patched Cemu for Milk Bar. Build it using Cemu's official BUILD.md instructions."
