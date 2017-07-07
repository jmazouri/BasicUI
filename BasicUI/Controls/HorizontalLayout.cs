using ImGuiNET;

namespace BasicUI.Controls
{
    public class HorizontalLayout : Container
    {
        protected override void InternalRender()
        {
            foreach (var child in Children)
            {
                child.Render();

                if (child != Children[Children.Count - 1])
                {
                    ImGui.SameLine();
                }
            }
        }

        public HorizontalLayout(string id = "") : base(id)
        {
        }
    }
}
