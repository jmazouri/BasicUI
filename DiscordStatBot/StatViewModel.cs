using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    public class StatViewModel : INotifyPropertyChanged
    {
        private DiscordSocketClient _client;

        public Dictionary<ulong, List<float>> MessageHistories { get; set; } = new Dictionary<ulong, List<float>>();

        private Dictionary<ulong, float> _lastSecondHistories { get; set; } = new Dictionary<ulong, float>();

        private Timer t;

        public StatViewModel(DiscordSocketClient client)
        {
            _client = client;

            _client.Ready += () => OnPropertyChanged(nameof(Servers));
            _client.GuildUpdated += (guild, guild2) => OnPropertyChanged(nameof(Servers));
            _client.GuildAvailable += guild => OnPropertyChanged(nameof(Servers));

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Servers")
                {
                    foreach (var server in Servers)
                    {
                        if (!_lastSecondHistories.ContainsKey(server.Id))
                        {
                            _lastSecondHistories.Add(server.Id, 0);
                        }
                    }
                }
            };

            t = new Timer(state =>
            {
                foreach (var kv in _lastSecondHistories)
                {
                    if (MessageHistories.ContainsKey(kv.Key))
                    {
                        MessageHistories[kv.Key].Add(kv.Value);
                    }
                    else
                    {
                        MessageHistories.Add(kv.Key, new List<float> { kv.Value });
                    }
                }
                
                _lastSecondHistories = Servers.ToDictionary(d => d.Id, d => 0f);

                MessageHistories = MessageHistories.ToDictionary
                (
                    d => d.Key,
                    d => d.Value.Count > 10 ? d.Value.Skip(1).ToList() : d.Value
                );

                OnPropertyChanged(nameof(MessageHistories));
            }, null, 0, 1000);

            _client.MessageReceived += msg =>
            {
                ulong guildid = (msg.Channel as SocketGuildChannel).Guild.Id;

                _lastSecondHistories[guildid] = _lastSecondHistories[guildid] + 1;

                return Task.CompletedTask;
            };
        }

        public IEnumerable<IGuild> Servers => _client.Guilds;

        public event PropertyChangedEventHandler PropertyChanged;
        protected Task OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return Task.CompletedTask;
        }
    }
}
