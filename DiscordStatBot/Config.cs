using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordStatBot
{
    public class Config
    {
        public string BotToken { get; set; }

        private Config() { }
        public static Config Load(string path = "config.json")
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
    }
}
