using BasicUI.Controls;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DiscordStatBot
{
    public class GuildListItem : Control
    {
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public List<float> Counts { get; set; } = new List<float>();

        public GuildListItem(string id = "") : base(id)
        {
        }

        protected override void InternalRender()
        {
            ImGui.PlotLines("", Counts.ToArray(), 0, "", 0, 6, new Vector2(100, 16), sizeof(float));
            ImGui.SameLine();
            ImGui.TextWrapped(GuildName);
        }
    }
}
