using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class SelectableList<T> : Repeater<T>
    {
        public Color Color { get; set; } = Color.White;
        public Action<T> OnClick { get; set; }

        public override void RenderItems()
        {
            if (Items.Count == 0) { return; } //Optimizations!

            ImGui.PushStyleColor(ColorTarget.Text, Color);
            foreach (var item in Items)
            {
                if (ImGui.Selectable(GetItemValue(item)))
                {
                    OnClick?.Invoke(item);
                }
            }
            ImGui.PopStyleColor();
        }

        public SelectableList(string id = "", Func<T, string> selector = null, Action<T> onClick = null) : base(id, selector)
        {
            OnClick = onClick;
        }
    }
}
