#include <mach-o/dyld.h>
#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

int main(int argc, char **argv) {
    setenv("PYTHONDONTWRITEBYTECODE", "1", 1);

    char executable[PATH_MAX];
    uint32_t size = sizeof(executable);
    if (_NSGetExecutablePath(executable, &size) != 0) {
        fputs("Hyrule Together: application path is too long\n", stderr);
        return 1;
    }

    char *slash = strrchr(executable, '/');
    if (!slash) return 1;
    *slash = '\0'; /* Contents/MacOS */
    slash = strrchr(executable, '/');
    if (!slash) return 1;
    *slash = '\0'; /* Contents */

    char python_home[PATH_MAX], python_path[PATH_MAX], qt_plugins[PATH_MAX];
    char framework_path[PATH_MAX * 2], python[PATH_MAX], script[PATH_MAX];
    snprintf(python_home, sizeof(python_home), "%s/Frameworks/Python.framework/Versions/Current", executable);
    snprintf(python_path, sizeof(python_path), "%s/Resources/python", executable);
    snprintf(qt_plugins, sizeof(qt_plugins), "%s/Resources/python/PySide6/Qt/plugins", executable);
    snprintf(framework_path, sizeof(framework_path), "%s/Resources/python/PySide6/Qt/lib:%s/Frameworks", executable, executable);
    snprintf(python, sizeof(python), "%s/bin/python3", python_home);
    snprintf(script, sizeof(script), "%s/Resources/app/milkbar_qt_gui.py", executable);

    setenv("PYTHONHOME", python_home, 1);
    setenv("PYTHONPATH", python_path, 1);
    setenv("QT_PLUGIN_PATH", qt_plugins, 1);
    setenv("DYLD_FRAMEWORK_PATH", framework_path, 1);

    char **child_argv = calloc((size_t)argc + 2, sizeof(char *));
    if (!child_argv) return 1;
    child_argv[0] = python;
    child_argv[1] = script;
    for (int i = 1; i < argc; ++i) child_argv[i + 1] = argv[i];
    execv(python, child_argv);
    perror("Hyrule Together");
    return 1;
}
