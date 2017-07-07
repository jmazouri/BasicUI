using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStatBot
{
    public class StatViewModel : INotifyPropertyChanged
    {
        private DiscordSocketClient _client;

        public StatViewModel(DiscordSocketClient client)
        {
            _client = client;

            _client.Ready += () => OnPropertyChanged(nameof(Servers));
            _client.GuildUpdated += (guild, guild2) => OnPropertyChanged(nameof(Servers));
            _client.GuildAvailable += guild => OnPropertyChanged(nameof(Servers));
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
