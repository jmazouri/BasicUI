using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class SelectableList<T> : Repeater<T>
    {
        public Action<T> OnClick { get; set; }

        public override void RenderItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (ImGui.Selectable(GetItemValue(item)))
                {
                    OnClick?.Invoke(item);
                }
            }
        }

        public SelectableList(string id = "", Func<T, string> selector = null, Action<T> onClick = null) : base(id, selector)
        {
            OnClick = onClick;
        }
    }
}
