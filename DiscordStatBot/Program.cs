using BasicUI;
using BasicUI.Controls;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    class Program
    {
        private Binder<StatViewModel> _binder;
        private StatViewModel _viewmodel;
        private Window _window;

        static void Main(string[] args)
        {
            new Program().Run().GetAwaiter().GetResult();
        }

        public async Task Run()
        {
            DiscordSocketClient client = new DiscordSocketClient();
            _viewmodel = new StatViewModel(client);
            _binder = new Binder<StatViewModel>(_viewmodel);

            _window = new Window(1280, 720, "Discord Stats");

            _window.RootContainer.Add(new Frame
            {
                Title = "Servers",
                Size = new Vector2(200, 250),
                Children =
                {
                    new InfiniteSpinner("guildsLoading"),
                    new WrapTextList<IGuild>("guilds", guild => guild.Name)
                }
            });

            _binder.BindSpecificPropChanged
            (
                vm => vm.Servers,
                _window.FindControlWithId<WrapTextList<IGuild>>("guilds"),
                (vm, list) => list.Items = vm.Servers.ToList()
            );

            _binder.BindSpecificPropChanged
            (
                vm => vm.Servers,
                _window.FindControlWithId<InfiniteSpinner>("guildsLoading"),
                (vm, load) => load.Enabled = !vm.Servers.Any()
            );

            await client.StartAsync();
            await client.LoginAsync(TokenType.User, "***REMOVED***");

            await _window.StartRenderAsync();
            //await Task.Delay(-1);
        }
    }
}