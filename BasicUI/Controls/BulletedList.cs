using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class BulletedList<T> : Repeater<T>
    {
        public BulletedList(string id = "", Func<T, string> selector = null) : base(id, selector)
        {
            Renderer = item => ImGui.BulletText(GetItemValue(item));
        }
    }
}
