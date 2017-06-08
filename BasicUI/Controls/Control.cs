using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace BasicUI.Controls
{
    public abstract class Control
    {
        public string Id { get; private set; }
        public Vector2 Position { get; set; } = Vector2.Zero;

        public abstract void Render();

        public Control(string id = "")
        {
            Id = id;
        }
    }
}
