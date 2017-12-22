using ImGuiNET;
using System;
using System.Numerics;

namespace BasicUI.Controls
{
    public abstract class ControlBase : IControl
    {
        public object BindingContext { get; set; }
        public T GetBinding<T>() where T : class
        {
            return BindingContext as T;
        }

        public string Id { get; protected set; }
        public Vector2 Position { get; set; } = Vector2.Zero;

        public Action<ControlBase> PreRender { get; set; }
        public Action<ControlBase> PostRender { get; set; }

        public void Render()
        {
            PreRender?.Invoke(this);
            //ImGui.PushID(Id);

            InternalRender();

            //ImGui.PopID();
            PostRender?.Invoke(this);
        }

        protected abstract void InternalRender();

        public ControlBase(string id = "")
        {
            Id = id;
        }
    }
}
