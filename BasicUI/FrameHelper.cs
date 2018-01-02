using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI
{
    public class FrameHelper : IDisposable
    {
        public FrameHelper(string title, WindowFlags flags = WindowFlags.Default)
        {
            ImGui.BeginWindow(title, flags);
        }

        public void Dispose()
        {
            ImGui.EndWindow();
        }
    }
}
