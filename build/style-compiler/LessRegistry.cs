using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StyleCompiler
{

    static class LessRegistry
    {

        public const string
            PUBLIC_NAME_DEFAULT = "DevExtreme",
            PUBLIC_NAME_EXPORTER = "DevExtreme Exporter";

        public const string
            MODULE_FRAMEWORK = "framework",
            MODULE_WIDGETS_BASE = "widgets-base",
            MODULE_TMP_WIDGETS_FOR_EXPORTER = "tmp-widgets-for-exporter",
            MODULE_TMP_EXPORTER = "tmp-exporter";

        public const string
            EULA_DEVEXTREME = "https://js.devexpress.com/Licensing/";

        public const string
            CSS_DISTRIBUTION_DEFAULT = "",
            CSS_DISTRIBUTION_SPA = "spa",
            CSS_DISTRIBUTION_EXPORTER = "exporter";

        public const string
            THEME_GENERIC = "generic",
            THEME_IOS7 = "ios7",
            THEME_ANDROID5 = "android5",
            THEME_WIN8 = "win8",
            THEME_WIN10 = "win10";

        public const string
            COLOR_SCHEME_DEFAULT = "default",
            COLOR_SCHEME_BLACK = "black",
            COLOR_SCHEME_WHITE = "white",
            COLOR_SCHEME_LIGHT = "light",
            COLOR_SCHEME_DARK = "dark",
            COLOR_SCHEME_CONTRAST = "contrast";

        public const string
            SIZE_SCHEME_DEFAULT = "default",
            SIZE_SCHEME_COMPACT = "compact";

        public static Dictionary<string, CssDistributionInfo> CssDistributions = new Dictionary<string, CssDistributionInfo> {
            {
                CSS_DISTRIBUTION_DEFAULT,
                new CssDistributionInfo {
                    LicenseInfo = EULA_DEVEXTREME,
                    Modules = new[] { MODULE_WIDGETS_BASE },
                    SupportedThemes = new[] { THEME_GENERIC, THEME_IOS7, THEME_ANDROID5, THEME_WIN8, THEME_WIN10 },

                    SupportedSizeSchemes = new Dictionary<string,string[]> {
                        { THEME_GENERIC, new[] { SIZE_SCHEME_DEFAULT, SIZE_SCHEME_COMPACT } }
                    }
                }
            },
            {
                CSS_DISTRIBUTION_SPA,
                new CssDistributionInfo {
                    LicenseInfo = EULA_DEVEXTREME,
                    Modules = new[] { MODULE_FRAMEWORK },
                    UseCommonPostfix = false
                }
            },
            {
#warning TODO deprecated in 16.1
                CSS_DISTRIBUTION_EXPORTER,
                new CssDistributionInfo {
                    PublicName = PUBLIC_NAME_EXPORTER,
                    LicenseInfo = EULA_DEVEXTREME,
                    Modules = new[] { MODULE_TMP_WIDGETS_FOR_EXPORTER, MODULE_TMP_EXPORTER },
                    SupportedThemes = new[] { THEME_GENERIC },

                    SupportedSizeSchemes = new Dictionary<string,string[]> {
                        { THEME_GENERIC, new[] { SIZE_SCHEME_DEFAULT } }
                    }
                }
            }
        };

        public static readonly IDictionary<string, KnownThemeInfo> KnownThemeMap = new KnownThemeInfo[] {
            new KnownThemeInfo {
                Name = THEME_IOS7,
                PublicName = THEME_IOS7,
                ColorSchemeNames = new [] { COLOR_SCHEME_DEFAULT }
            },
            new KnownThemeInfo {
                Name = THEME_ANDROID5,
                PublicName = THEME_ANDROID5,
                ColorSchemeNames = new[] { COLOR_SCHEME_LIGHT }
            },
            new KnownThemeInfo {
                Name = THEME_WIN8,
                PublicName = THEME_WIN8,
                ColorSchemeNames = new[] { COLOR_SCHEME_BLACK, COLOR_SCHEME_WHITE }
            },
            new KnownThemeInfo {
                Name = THEME_GENERIC,
                PublicName = string.Empty,
                ColorSchemeNames = new[] { COLOR_SCHEME_LIGHT, COLOR_SCHEME_DARK, COLOR_SCHEME_CONTRAST }
            },
            new KnownThemeInfo {
                Name = THEME_WIN10,
                PublicName = THEME_WIN10,
                ColorSchemeNames = new[] { COLOR_SCHEME_BLACK, COLOR_SCHEME_WHITE }
            }
        }.ToDictionary(i => i.Name);

        static string[] GenerateWidgetsLessFileList(string themeName)
        {
            var names = Enumerable.Empty<string>();

            if (themeName == "common")
            {
                names = names.Concat(new[] { "../ui" });
            }
            else
            {
                if (themeName != "base")
                    names = names.Concat(new[] { "typography", "common" });
            }

            names = names.Concat(new[] {
                "icons",
                "widget",
                "badge",
                "draggable",
                "resizable",
                "box",
                "responsiveBox",
                "button",
                "scrollable",
                "scrollView",
                "checkBox",
                "switch",
                "tabs",
                "map",
                "navBar",
                "textEditor",
                "textBox",
                "dropDownEditor",
                "list",
                "dropDownList",
                "textArea",
                "numberBox",
                "dateBox",
                "dateView",
                "toolbar",
                "tileView",
                "overlay",
                "toast",
                "popup",
                "popover",
                "trackBar",
                "progressBar",
                "tooltip",
                "slider",
                "rangeSlider",
                "gallery",
                "lookup",
                "actionSheet",
                "loadIndicator",
                "loadPanel",
                "autocomplete",
                "dropDownMenu",
                "selectBox",
                "tagBox",
                "radioButton",
                "radioGroup",
                "pivotTabs",
                "pivot",
                "panorama",
                "accordion",
                "slideOutView",
                "slideOut",
                "pager",
                "colorView",
                "colorBox",
                "gridBase",
                "dataGrid",
                "pivotGrid",
                "treeList",
                "menuBase",
                "menu",
                "contextMenu",
                "calendar",
                "multiView",
                "treeView",
                "fieldset",
                "tabPanel",
                "fileUploader",
                "validation",
                "timeView",
                "scheduler",
                "form",
                "spa"
            });

            // Non-themeable components that have only common styles
            if(themeName == "common") {
                names = names.Concat(new[] {
                    "deferRendering"
                });
            }

            return (new string[] { "../../mixins.less" })
                .Concat(names.Select(name =>
                {
                    if (themeName != "common" && themeName != "base")
                        name += "." + themeName;
                    name = name + ".less";
                    return name;
                }))
                .ToArray();
        }

        static readonly Dictionary<string, ModuleInfo> Modules = new Dictionary<string, ModuleInfo> {
            {
                MODULE_FRAMEWORK,
                new ModuleInfo {
                    PublicName = PUBLIC_NAME_DEFAULT + " (Single Page App Framework)",
                    LicenseInfo = EULA_DEVEXTREME,
                    StyleInfo = new ModuleStyleInfo {
                        LessRoot = "framework",
                        CommonLessFiles = new[] {
                            "framework.less"
                        }
                    }
                }
            },
            {
                MODULE_WIDGETS_BASE,
                new ModuleInfo {
                    PublicName = PUBLIC_NAME_DEFAULT + " (Common Widgets)",
                    LicenseInfo = EULA_DEVEXTREME,
                    StyleInfo = new ModuleStyleInfo {
                        LessRoot = "widgets",
                        CommonLessFiles = GenerateWidgetsLessFileList("common"),
                        BaseLessFiles = GenerateWidgetsLessFileList("base"),
                        Themes = KnownThemeMap.Values.Select(i => new ModuleStyleThemeInfo {
                            Name = i.Name,
                            ColorSchemeNames = i.ColorSchemeNames,
                            LessFiles = GenerateWidgetsLessFileList(i.Name)
                        }).ToArray()
                    }
                }
            },


#warning TODO deprecated in 16.1
            {
                MODULE_TMP_WIDGETS_FOR_EXPORTER,
                new ModuleInfo {
                    StyleInfo = new ModuleStyleInfo {
                        LessRoot = "widgets",
                        CommonLessFiles = new[] {
                            "../../mixins.less",
                            "menuBase.less",
                            "contextMenu.less",
                            "menu.less",
                            "overlay.less",
                            "widget.less"
                        },
                        Themes = new[] {
                            new ModuleStyleThemeInfo {
                                Name = THEME_GENERIC,
                                ColorSchemeNames = new[] { COLOR_SCHEME_LIGHT, COLOR_SCHEME_DARK, COLOR_SCHEME_CONTRAST },
                                LessFiles = new[] {
                                    "../../mixins.less",
                                    "../base/icons.less",
                                    "common.generic.less",
                                    "icons.generic.less",
                                    "menuBase.generic.less",
                                    "contextMenu.generic.less",
                                    "menu.generic.less"
                                }
                            }
                        }
                    }
                }
            },
            {
                MODULE_TMP_EXPORTER,
                new ModuleInfo {
                    LicenseInfo = EULA_DEVEXTREME,
                    StyleInfo = new ModuleStyleInfo {
                        LessRoot = "exporter",
                        CommonLessFiles = new string[] { },
                        Themes = new[]{
                            new ModuleStyleThemeInfo {
                                Name = THEME_GENERIC,
                                ColorSchemeNames = new[] { COLOR_SCHEME_LIGHT, COLOR_SCHEME_DARK, COLOR_SCHEME_CONTRAST },
                                LessFiles = new [] {
                                    "exporter.generic.less"
                                }
                            }
                        }
                    }
                }
            }
        };


        public class CssDistributionInfo
        {
            public string PublicName = PUBLIC_NAME_DEFAULT;
            public string LicenseInfo = "For internal use only";
            public string[] Modules;
            public string[] SupportedThemes;
            public Dictionary<string, string[]> SupportedSizeSchemes;
            public bool ForceCommonsInExternalFiles = false;
            public bool UseCommonPostfix = true;

            public bool CommonsInExternalFiles
            {
                get { return ForceCommonsInExternalFiles || SupportedThemes == null || SupportedThemes.Length > 1; }
            }

        }

        public abstract class BaseThemeInfo
        {
            public string Name;
            public string[] ColorSchemeNames = new string[0];
        }

        public class KnownThemeInfo : BaseThemeInfo
        {
            public string PublicName;
        }

        public class ModuleStyleThemeInfo : BaseThemeInfo
        {
            public string[] LessFiles = new string[0];
        }

        public class ModuleStyleInfo
        {
            public string LessRoot;
            public string[] CommonLessFiles;
            public string[] BaseLessFiles;
            public ModuleStyleThemeInfo[] Themes;
        }

        public class ModuleInfo
        {
            public string PublicName = null;
            public string LicenseInfo = "For internal use only";
            public ModuleStyleInfo StyleInfo;
            public string[] RequireModules = new string[0];
        }


        public static string GetIconsPath(string sourcePath)
        {
            return Path.Combine(sourcePath, "../icons");
        }

        public static bool IsDefaultSizeScheme(string value)
        {
            return String.IsNullOrEmpty(value) || value == SIZE_SCHEME_DEFAULT;
        }

        public static bool SizeSchemesEqual(string x, string y)
        {
            return IsDefaultSizeScheme(x) && IsDefaultSizeScheme(y) || x == y;
        }

        public static string[] GetThemeLessFiles(string sourcePath, string moduleName, string themeName, string colorSchemeName, string sizeSchemeName)
        {
            var styleInfo = Modules[moduleName].StyleInfo;
            if (styleInfo == null || styleInfo.Themes == null)
                return null;

            var themeInfo = styleInfo.Themes.FirstOrDefault(t => t.Name == themeName);
            if (themeInfo == null)
                return null;

            var lessFiles = new List<string>();

            var colorSchemeBasePart = String.Format("{0}/{1}/color-schemes/{2}/{1}.{2}", styleInfo.LessRoot, themeName, colorSchemeName);
            lessFiles.Add(colorSchemeBasePart + ".less");
            lessFiles.Add(colorSchemeBasePart + ".icons.less");

            if (!String.IsNullOrEmpty(sizeSchemeName))
            {
                lessFiles.Add(String.Format("{0}/{1}/size-schemes/{2}.less", styleInfo.LessRoot, themeName, sizeSchemeName));
            }

            if (styleInfo.BaseLessFiles != null)
            {
                foreach (var item in styleInfo.BaseLessFiles)
                {
                    var path = EliminateDoubleDots(styleInfo.LessRoot + "/base/" + item);

                    if (File.Exists(Path.Combine(sourcePath, path)))
                        lessFiles.Add(path);
                }
            }

            foreach (var item in themeInfo.LessFiles)
                lessFiles.Add(EliminateDoubleDots(styleInfo.LessRoot + "/" + themeName + "/" + item));

            return lessFiles.ToArray();
        }

        public static string[] GetCommonLessFiles(string[] moduleNames)
        {
            var list = new List<string>();

            foreach (var moduleName in moduleNames)
            {
                var styleInfo = Modules[moduleName].StyleInfo;
                if (styleInfo == null)
                    continue;

                foreach (var item in styleInfo.CommonLessFiles)
                    list.Add(EliminateDoubleDots(styleInfo.LessRoot + "/common/" + item));
            }

            return list.ToArray();
        }

        static string EliminateDoubleDots(string lessPath)
        {
            return new Uri("http://test/" + lessPath).LocalPath.TrimStart('/');
        }

        public static string[] ResolveRequiredModules(params string[] desiredModuleNames)
        {
            var trace = new HashSet<string>();
            var result = new List<string>();
            foreach (var item in desiredModuleNames)
                ResolveRequiredModulesCore(item, result, trace);
            return result.ToArray();
        }

        static void ResolveRequiredModulesCore(string moduleName, List<string> result, ISet<string> trace)
        {
            if (trace.Contains(moduleName))
                return;

            var moduleObj = Modules[moduleName];
            if (moduleObj == null)
                return;

            trace.Add(moduleName);
            foreach (var item in moduleObj.RequireModules)
                ResolveRequiredModulesCore(item, result, trace);
            result.Add(moduleName);
        }

    }

}
