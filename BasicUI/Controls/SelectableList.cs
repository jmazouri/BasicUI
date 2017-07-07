using ImGuiNET;
using System;

namespace BasicUI.Controls
{
    public class SelectableList<T> : Repeater<T>
    {
        public Action<T> OnClick { get; set; }
        public Action<T> OnDoubleClick { get; set; }
        public Action<T> OnRightClick { get; set; }

        public SelectableList(string id = "", Func<T, string> selector = null, Action<T> onClick = null) : base(id, selector)
        {
            OnClick = onClick;
            Renderer = item =>
            {
                if (ImGui.Selectable(GetItemValue(item)))
                {
                    OnClick?.Invoke(item);

                    if (ImGui.IsMouseDoubleClicked(0))
                    {
                        OnDoubleClick?.Invoke(item);
                    }

                    if (ImGui.IsMouseClicked(1))
                    {
                        OnRightClick?.Invoke(item);
                    }
                }
            };
        }
    }
}
