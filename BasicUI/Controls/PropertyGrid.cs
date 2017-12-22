using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicUI.Controls
{
    public class PropertyGrid : ControlBase
    {
        public bool Border { get; set; }
        public Dictionary<string, Func<string>> Properties { get; set; } = new Dictionary<string, Func<string>>();

        protected override void InternalRender()
        {
            ImGui.Columns(2, Id, Border);

            foreach (var prop in Properties)
            {
                ImGui.TextDisabled(prop.Key);
                ImGui.NextColumn();
                ImGui.Text(prop.Value());

                if (!prop.Equals(Properties.Last()))
                {
                    ImGui.NextColumn();
                }
            }

            ImGui.Columns(1, "", false);
        }

        public PropertyGrid(string id = "") : base(id) { }
    }
}
