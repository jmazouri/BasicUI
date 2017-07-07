using BasicUI.Native;
using ImageSharp;
using ImageSharp.PixelFormats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasicUI.FileBrowser
{
    public static class IconLoader
    {
        private static Dictionary<string, string[]> _iconMap = new Dictionary<string, string[]>();

        public static void LoadAll(string iconFolder)
        {
            if (File.Exists("iconmap.json"))
            {
                string iconMap = File.ReadAllText("iconmap.json");
                _iconMap = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(iconMap);
            }

            foreach (string filePath in Directory.GetFiles(iconFolder, "*.png"))
            {
                string fileType = Path.GetFileNameWithoutExtension(filePath);

                ImageHelper.LoadImage(fileType, Image.Load<Argb32>(filePath));

                if (_iconMap.ContainsKey(fileType))
                {
                    foreach (string ext in _iconMap[fileType])
                    {
                        ImageHelper.LoadImage(ext, Image.Load<Argb32>(filePath));
                    }
                }
            }
        }
    }
}
