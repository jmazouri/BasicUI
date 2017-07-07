using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class InfiniteSpinner : Control
    {
        public bool Enabled { get; set; }
        public int Width { get; set; } = 24;

        private int _ind;
        private int LoopingIndex
        {
            get
            {
                return _ind;
            }
            set
            {
                _ind = value;

                if (_ind >= Width)
                {
                    _ind = 0;
                }
            }
        }

        protected override void InternalRender()
        {
            if (Enabled)
            {
                StringBuilder build = new StringBuilder();

                for (int i = 0; i < Width; i++)
                {
                    if (i == LoopingIndex)
                    {
                        build.Append('-');
                    }
                    else
                    {
                        build.Append('_');
                    }
                }

                ImGui.Text(build.ToString());

                LoopingIndex++;
            }
        }

        public InfiniteSpinner(string id = "") : base(id) { }
    }
}
