using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class Button : Control
    {
        public string Text { get; set; }
        public bool IsSmall { get; set; }

        public Action<Button> OnClick { get; set; }

        public override void Render()
        {
            if (IsSmall)
            {
                if (ImGui.SmallButton(Text))
                {
                    OnClick(this);
                }
            }
            else
            {
                if (ImGui.Button(Text))
                {
                    OnClick(this);
                }
            }
        }

        public Button(string id = "") : base(id) { }
    }
}
