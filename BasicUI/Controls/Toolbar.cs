using ImGuiNET;
using System.Collections.Generic;
using System.Collections;

namespace BasicUI.Controls
{
    public class Toolbar : ControlBase, IEnumerable<IMenuItem>
    {
        private ControlCollection<IMenuItem> Items { get; set; }

        protected override void InternalRender()
        {
            if (Items.Count == 0) { return; }

            if (ImGui.BeginMainMenuBar())
            {
                foreach (MenuItem item in Items)
                {
                    item.Render();
                }

                ImGui.EndMainMenuBar();
            }
        }

        public void Add(IMenuItem item) => Items.Add(item);
        public void AddRange(IEnumerable<IMenuItem> items) => Items.AddRange(items);
        
        public IEnumerator<IMenuItem> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        internal Toolbar(string id = "") : base(id)
        {
            Items = new ControlCollection<IMenuItem>(this);
        }
    }
}
