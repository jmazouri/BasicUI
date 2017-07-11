using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace BasicUI.Controls
{
    public class ProgressBar : Control
    {
        public float Progress { get; set; }
        public string Overlay { get; set; }

        private Vector2 _size;
        public Vector2 Size
        {
            get => _size;
            set => _size = value;
        }

        protected override unsafe void InternalRender()
        {
            fixed (Vector2* size = &_size)
            {
                ImGuiNative.igProgressBar(Progress, size, Overlay);
            }
        }

        public ProgressBar(string id = "") : base(id) { }
    }
}
