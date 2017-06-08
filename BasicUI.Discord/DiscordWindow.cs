using BasicUI.Controls;
using Discord;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BasicUI.DiscordClient
{
    public class DiscordWindow
    {
        public static Window Create(Action<IGuild> guildSelected, Action<IChannel> channelSelected)
        {
            Window w = new Window(windowTitle: "BasicDiscord")
            {
                FontPath = "Fonts/DroidSans.ttf"
            };

            

            w.RootContainer.Add(new Frame("serverList")
            {
                Title = "Servers",
                Size = new Vector2(180, 150),
                Position = new Vector2(0, 0),
                WindowFlags = WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoTitleBar,
                Children =
                {
                    new SelectableList<IGuild>("guilds", guild => guild.Name, guildSelected)
                }
            });

            w.RootContainer.Add(new Frame("channelList")
            {
                Title = "Channels",
                Size = new Vector2(180, 330),
                Position = new Vector2(0, 150),
                WindowFlags = WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoTitleBar,
                Children =
                {
                    new SelectableList<IChannel>("channels", channel => channel.Name, channelSelected)
                }
            });

            w.RootContainer.Add(new Frame("messageList")
            {
                Title = "Messages",
                Size = new Vector2(450, 480),
                Position = new Vector2(190, 0),
                BackgroundAlpha = 0.66f,
                WindowFlags = WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoCollapse | WindowFlags.NoSavedSettings,
                Children =
                {
                    new WrapTextList<IMessage>("messages", (msg) => $"[{msg.Author.Username}] {msg.Content}")
                    {
                        ScrollToBottom = true
                    }
                }
            });

            return w;
        }
    }
}
