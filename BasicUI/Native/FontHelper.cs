using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasicUI.Native
{
    public static class FontHelper
    {
        static readonly string WindowsFontDir = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), "fonts");

        public static void LoadFont(string fontPath = null, int size = 14)
        {
            if (fontPath == null)
            {
                ImGui.GetIO().FontAtlas.AddDefaultFont();
            }

            if (String.IsNullOrWhiteSpace(fontPath) || !File.Exists(fontPath))
            {
                return;
            }

            ImGui.GetIO().FontAtlas.AddFontFromFileTTF(fontPath, size);
        }

        public static string LocateFont(string fileName)
        {
            if (fileName == null || Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            if (Directory.Exists(WindowsFontDir))
            {
                string windowsFontPath = Path.Combine(WindowsFontDir, fileName);

                if (File.Exists(windowsFontPath))
                {
                    return windowsFontPath;
                }

                if (File.Exists(fileName))
                {
                    return fileName;
                }

                return null;
            }

            throw new NotImplementedException($"Can't locate font \"{fileName}\".");
        }
    }
}
