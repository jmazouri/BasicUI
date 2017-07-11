using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BasicUI.Controls
{
    public abstract class Plot : Control
    {
        public IEnumerable<float> Points { get; set; } = new List<float>();

        public string Label { get; set; } = "";
        public string Overlay { get; set; } = "";
        public Vector2 Size { get; set; }

        public float ScaleMin { get; set; } = 0;
        public float ScaleMax { get; set; } = 1;

        public bool AutoScale { get; set; } = false;

        public Plot(string id = "") : base(id) { }

        protected override void InternalRender()
        {
            if (AutoScale)
            {
                ScaleMax = Points.Any() ? Points.Max() : ScaleMax;
            }
        }
    }
}