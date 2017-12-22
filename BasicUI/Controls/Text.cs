using ImGuiNET;

namespace BasicUI.Controls
{
    public class Text : ControlBase
    {
        public string Content { get; set; }
        public Color Color { get; set; } = Color.White;

        protected override void InternalRender()
        {
            if (Position.X != 0)
            {
                ImGuiNative.igSetCursorPosX(Position.X);
            }

            ImGui.Text(Content, Color);
        }

        public Text(string id = "") : base(id) { }
    }
}
