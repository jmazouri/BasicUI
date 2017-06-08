using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class WrapTextList<T> : Repeater<T>
    {
        public override void RenderItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                ImGui.TextWrapped(GetItemValue(item));
            }
        }

        public WrapTextList(string id = "", Func<T, string> selector = null) : base(id, selector) { }
    }
}
