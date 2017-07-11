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
        private Config _config;
        private StatViewModel _viewmodel;

        private Window _window;
        private Binder<StatViewModel> _binder;

        static void Main(string[] args)
        {
            new Program().Run().GetAwaiter().GetResult();
        }

        public async Task Run()
        {
            _config = Config.Load();

            DiscordSocketClient client = new DiscordSocketClient();
            _viewmodel = new StatViewModel(client);
            _binder = new Binder<StatViewModel>(_viewmodel);

            _window = new Window(1280, 720, "Discord Stats");

            _window.RootContainer.Add(new Frame
            {
                Title = "Servers",
                Size = new Vector2(300, 250),
                Children =
                {
                    new InfiniteSpinner("guildsLoading"),
                    new Container("guilds"),
                    new BarPlot("messagePlot")
                    {
                        Overlay = "Messages / second",
                        Size = new Vector2(250, 100),
                        AutoScale = true
                    }
                }
            });

            _binder.BindSpecificPropChanged
            (
                vm => vm.Servers,
                _window.FindControlWithId<InfiniteSpinner>("guildsLoading"),
                (vm, load) => load.Enabled = !vm.Servers.Any()
            );

            _binder.BindSpecificPropChanged
            (
                vm => vm.MessageHistories,
                _window.FindControlWithId<BarPlot>("messagePlot"),
                (vm, plot) => plot.Points = vm.MessageHistories.Select(d => d.Value.Sum())
            );

            _binder.BindSpecificPropChanged
            (
                vm => vm.MessageHistories,
                _window.FindControlWithId<Container>("guilds"),
                (vm, container) =>
                {
                    container.Clear();

                    foreach (var hist in vm.MessageHistories)
                    {
                        container.Add(new GuildListItem
                        {
                            GuildName = client.GetGuild(hist.Key).Name,
                            Counts = hist.Value
                        });
                    }
                }
            );

            await client.StartAsync();
            await client.LoginAsync(TokenType.User, _config.BotToken);

            await _window.StartRenderAsync();
            //await Task.Delay(-1);
        }
    }
}