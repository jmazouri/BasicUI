using System;
using System.Numerics;

namespace BasicUI.Controls
{
    public abstract class Control
    {
        public object BindingContext { get; set; }
        public T GetBinding<T>() where T : class
        {
            return BindingContext as T;
        }

        /// <summary>
        /// The unique identifier for the control. Some controls require this to be unique in order to render properly.
        /// </summary>
        public string Id { get; protected set; }
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
