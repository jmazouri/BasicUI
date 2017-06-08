using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class WrapTextList<T> : Repeater<T>
    {
        public Color Color { get; set; } = Color.White;

        public override void RenderItems()
        {
            if (Items.Count == 0) { return; } //Optimizations!

            ImGui.PushStyleColor(ColorTarget.Text, Color);
            foreach (var item in Items)
            {
                ImGui.TextWrapped(GetItemValue(item));
            }
            ImGui.PopStyleColor();
        }

        public WrapTextList(string id = "", Func<T, string> selector = null) : base(id, selector) { }
    }
}
