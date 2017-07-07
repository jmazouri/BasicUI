using BasicUI.Controls;
using Discord;
using Discord.WebSocket;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BasicUI.DiscordClient
{
    public class DiscordWindow
    {
        public static Window Create(Action<string> messageSubmit, Action<IGuild> guildSelected, Action<IChannel> channelSelected)
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
                Size = new Vector2(450, 440),
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

            w.RootContainer.Add(new Frame("inputBox")
            {
                Size = new Vector2(450, 20),
                Position = new Vector2(190, 440),
                BackgroundAlpha = 0.5f,
                WindowFlags = WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoCollapse | WindowFlags.NoSavedSettings | WindowFlags.NoTitleBar,
                Children =
                {
                    new TextBox("messageInput")
                    {
                        Label = "Message",
                        InputTextFlags = InputTextFlags.EnterReturnsTrue
                    }
                }
            });

            var inputBox = w.FindControlWithId<TextBox>("messageInput");
            inputBox.PropertyChanged += (sender, text) => messageSubmit(inputBox.Text);

            return w;
        }
    }
}
