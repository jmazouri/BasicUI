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

        public Action<Control> PreRender { get; set; }
        public Action<Control> PostRender { get; set; }

        public void Render()
        {
            PreRender?.Invoke(this);
            InternalRender();
            PostRender?.Invoke(this);
        }

        protected abstract void InternalRender();

        public Control(string id = "")
        {
            Id = id;
        }
    }
}
