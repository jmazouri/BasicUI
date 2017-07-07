using ImGuiNET;

namespace BasicUI.Controls
{
    public class ModalPrompt : Container
    {
        private bool _open = false;
        public bool IsOpen
        {
            get
            {
                return _open;
            }
            set
            {
                _open = value;
            }
        }

        protected override void InternalRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Id);
                ImGui.SetNextWindowPosCenter(SetCondition.Appearing);
            }

            if (ImGui.BeginPopupModal(Id, WindowFlags.AlwaysAutoResize))
            {
                if (!IsOpen)
                {
                    ImGui.CloseCurrentPopup();
                }

                base.InternalRender();
                ImGui.EndPopup();
            }
        }

        public ModalPrompt(string id = "msgBox") : base(id)
        {
            if (id == "msgBox")
            {
                Id = Id + Window.GlobalTime;
            }
        }
    }
}
