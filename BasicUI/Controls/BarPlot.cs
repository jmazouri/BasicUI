using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicUI.Controls
{
    public class BarPlot : Plot
    {
        protected override void InternalRender()
        {
            base.InternalRender();

            if (Points.Any())
            {
                ImGui.PlotHistogram(Label, Points.ToArray(), 0, Overlay, ScaleMin, ScaleMax, Size, sizeof(float));
            }
        }

        public BarPlot(string id = "") : base(id) { }
    }
}
