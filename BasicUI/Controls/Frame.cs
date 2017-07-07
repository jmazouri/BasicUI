using ImGuiNET;
using System;

namespace BasicUI.Controls
{
    public class Frame : Container
    {
        public string Title { get; set; } = String.Empty;
        public bool StartCentered { get; set; }

        private bool _opened;
        public bool IsOpen => _opened;
        
        public WindowFlags WindowFlags { get; set; }

        private float _bgAlpha = 1;
        public float BackgroundAlpha
        {
            get => _bgAlpha;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _bgAlpha = value;
                }
            }
        }

        protected override void InternalRender()
        {
            ImGui.SetNextWindowPos(Position, SetCondition.Appearing);
            ImGui.SetNextWindowSize(Size, SetCondition.Appearing);

            if (StartCentered)
            {
                ImGui.SetNextWindowPosCenter(SetCondition.Appearing);
            }

            ImGui.BeginWindow(Title, ref _opened, Size, BackgroundAlpha, WindowFlags);

            base.InternalRender();

            ImGui.EndWindow();
        }

        public Frame(string id = "") : base(id) { }
    }
}
