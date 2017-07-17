using BasicUI;
using BasicUI.Controls;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace DiscordStatBot
{
    public static class StatWindow
    {
        public static void Setup(Window window, Binder<StatViewModel> binder)
        {
            window.RootContainer.Add(new Frame
            {
                Title = "Servers",
                Size = new Vector2(400, 300),
                Children =
                {
                    new InfiniteSpinner("guildsLoading") { Speed = 0.025f },
                    new Container("guilds")
                }
            });

            window.RootContainer.Add(new Frame
            {
                Title = "Message Rate",
                Size = new Vector2(265, 135),
                Position = new Vector2(410, 0),
                WindowFlags = WindowFlags.NoResize,
                Children =
                {
                    new BarPlot("messagePlot")
                    {
                        Overlay = "Messages / second",
                        Size = new Vector2(250, 100),
                        AutoScale = true
                    }
                }
            });

            window.RootContainer.Add(new Frame
            {
                Title = "Word Counts",
                Size = new Vector2(200, 400),
                Position = new Vector2(685, 0),
                Children =
                {
                    new Container("wordCounts")
                }
            });

            binder.BindSpecificPropChanged
            (
                vm => vm.Servers,
                window.FindControlWithId<InfiniteSpinner>("guildsLoading"),
                (vm, load) => load.Enabled = !vm.Servers.Any()
            );

            binder.BindPreRender
            (
                window.FindControlWithId<Container>("wordCounts"),
                (vm, container) =>
                {
                    foreach (var word in vm.WordCounts.SelectMany(d=>d.Value).OrderByDescending(d => d.Value).Take(25))
                    {
                        ImGui.Text($"{word.Key} - {word.Value}");
                    }
                }
            );

            binder.BindSpecificPropChanged
            (
                vm => vm.MessageHistories,
                window.FindControlWithId<BarPlot>("messagePlot"),
                (vm, plot) => plot.Points =       
                    vm.MessageHistories.Count == 0
                        ? new List<float>()
                        : Enumerable.Range(0, vm.MessageHistories.First().Value.Count)
                                .Select(i => vm.MessageHistories.Select(kv => vm.MessageHistories[kv.Key][i]).Sum())
            );

            binder.BindPreRender
            (
                window.FindControlWithId<Container>("guilds"),
                (vm, container) =>
                {
                    foreach (var hist in vm.MessageHistories)
                    {
                        ImGui.PlotLines("", hist.Value.ToArray(), 0, "", 0, 6, new Vector2(100, 16), sizeof(float));
                        ImGui.SameLine();
                        ImGui.TextWrapped(vm.Client.GetGuild(hist.Key).Name);
                    }
                }
            );
        }
    }
}
