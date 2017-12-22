using ImGuiNET;
using System;
using System.Numerics;

namespace BasicUI.Controls
{
    public class Button : ControlBase
    {
        public string Text { get; set; }
        public Vector2 Size { get; set; }

        public bool IsSmall { get; set; }
        public bool IsInvisible { get; set; }

        public Action<Button> OnClick { get; set; }

        protected override void InternalRender()
        {
            if (IsInvisible)
            {
                if (ImGui.InvisibleButton(Id, Size))
                {
                    OnClick?.Invoke(this);
                }
            }
            else if (IsSmall)
            {
                if (ImGui.SmallButton(Text))
                {
                    OnClick?.Invoke(this);
                }
            }
            else
            {
                if (ImGui.Button(Text, Size))
                {
                    OnClick?.Invoke(this);
                }
            }
        }

        public Button(string id = "") : base(id) { }
    }
}
