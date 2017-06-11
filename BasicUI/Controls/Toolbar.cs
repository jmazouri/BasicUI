using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class Toolbar : Control
    {
        protected override void InternalRender()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Edit"))
                {
                    ImGui.MenuItem("Test");
                    ImGui.Separator();
                    ImGui.MenuItem("Other Test");

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }
        }
        
        internal Toolbar(string id = "") : base(id) { }
    }
}
