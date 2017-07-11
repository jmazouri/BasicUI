using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicUI.Controls
{
    public class LinePlot : Plot
    {
        protected override void InternalRender()
        {
            base.InternalRender();

            if (Points.Any())
            {
                ImGui.PlotLines(Label, Points.ToArray(), 0, Overlay, ScaleMin, ScaleMax, Size, sizeof(float));
            }
        }

        public LinePlot(string id = "") : base(id) { }
    }
}