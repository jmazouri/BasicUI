using ImGuiNET;

namespace BasicUI.Controls
{
    public class Panel : Container
    {
        public bool Bordered { get; set; } = true;
        public WindowFlags WindowFlags { get; set; }

        protected override void InternalRender()
        {
            ImGui.BeginChild(Id, Size, Bordered, WindowFlags);

            base.InternalRender();

            ImGui.EndChild();
        }

        public Panel(string id = "") : base(id) { }
    }
}
