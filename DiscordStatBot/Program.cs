using BasicUI;
using BasicUI.Controls;
using Discord;
using Discord.WebSocket;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    class Program
    {
        private StatViewModel _viewmodel;

        private Binder<StatViewModel> _binder;

        static void Main(string[] args)
        {
            new Program().Run(Config.Load()).GetAwaiter().GetResult();
        }

        public async Task Run(Config config)
        {
            DiscordSocketClient client = new DiscordSocketClient();
            _viewmodel = new StatViewModel(client);
            _binder = new Binder<StatViewModel>(_viewmodel);

            var window = new Window(475, 500, "Discord Stats");

            StatWindow.Setup(window, _binder);

            await client.StartAsync();
            await client.LoginAsync(TokenType.User, config.BotToken);

            await window.StartRenderAsync();
            //await Task.Delay(-1);
        }
    }
}