﻿using ImGuiNET;
using System;

namespace BasicUI.Controls
{
    public class WrapTextList<T> : Repeater<T>
    {
        public WrapTextList(string id = "", Func<T, string> selector = null) : base(id, selector)
        {
            Renderer = item => ImGui.TextWrapped(GetItemValue(item));
        }
    }
}
