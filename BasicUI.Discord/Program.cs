using System;
using BasicUI;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using BasicUI.Controls;
using System.Numerics;
using System.Linq;

namespace BasicUI.DiscordClient
{
    class Program
    {
        private DiscordSocketClient client;
        private static Window w;

        private static ulong _selectedGuild;
        private static ulong _selectedChannel;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new Program().Start().GetAwaiter().GetResult();
        }

        async Task Start()
        {
            w = DiscordWindow.Create
            (
                async (guild) =>
                {
                    _selectedGuild = guild.Id;
                    await LoadGuildChannels();
                },
                async (channel) =>
                {
                    _selectedChannel = channel.Id;
                    await UpdateMessageList();
                }
            );

            client = new DiscordSocketClient();
            await client.LoginAsync(TokenType.User, Config.Load().BotToken);
            await client.StartAsync();

            client.MessageReceived += Client_MessageReceived;

            client.Ready += async () =>
            {
                if (_selectedGuild == 0)
                {
                    _selectedGuild = client.Guilds.First().Id;
                }

                await LoadGuildChannels();

                w.FindControlWithId<Repeater<IGuild>>("guilds")
                    .AddRange(client.Guilds);
            };

            w.StartRenderThread();

            await Task.Delay(-1);
        }

        private async Task LoadGuildChannels()
        {
            var control = w.FindControlWithId<Repeater<IChannel>>("channels");
            var channels = client.GetGuild(_selectedGuild).Channels.Where(d => d is SocketTextChannel);

            control.Clear();
            control.AddRange(channels);

            _selectedChannel = channels.First().Id;
            await UpdateMessageList();
        }

        private async Task UpdateMessageList()
        {
            var guild = client.GetGuild(_selectedGuild);
            var channels = guild.Channels;
            var list = w.FindControlWithId<Repeater<IMessage>>("messages");

            list.Clear();
            w.FindControlWithId<Frame>("messageList").Title = $"{guild.Name} - {channels.First(d => d.Id == _selectedChannel).Name}";

            var messages = await (guild.GetChannel(_selectedChannel) as SocketTextChannel).GetMessagesAsync().Flatten();
            list.AddRange(messages.Reverse());
        }

        private Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg.Channel.Id != _selectedChannel) { return Task.CompletedTask; }

            var list = w.FindControlWithId<Repeater<IMessage>>("messages");
            list.Add(arg);

            return Task.CompletedTask;
        }
    }
}