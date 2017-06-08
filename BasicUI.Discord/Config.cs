using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasicUI.DiscordClient
{
    public class Config
    {
        public string BotToken { get; set; }

        public static Config Load(string path = "config.json")
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
    }
}
