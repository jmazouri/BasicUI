using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class Separator : MenuItem
    {
        protected override void InternalRender()
        {
            ImGui.Separator();
        }

        public Separator(string id = "") : base(id)
        {
        }
    }
}
