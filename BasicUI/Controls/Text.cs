﻿using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BasicUI.Controls
{
    public class Text : Control
    {
        public string Content { get; set; }
        public Color Color { get; set; } = Color.White;

        public override void Render()
        {
            ImGui.Text(Content, Color);
        }

        public Text(string id = "") : base(id) { }
    }
}