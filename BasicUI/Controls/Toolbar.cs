using ImGuiNET;
using System.Collections.Generic;
using System.Collections;

namespace BasicUI.Controls
{
    public class Toolbar : Control, IEnumerable<MenuItem>
    {
        private ControlCollection<MenuItem> Items { get; set; }

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

        public void Add(MenuItem item) => Items.Add(item);
        public void AddRange(MenuItem item) => Items.AddRange(item);
        public IEnumerator<MenuItem> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        internal Toolbar(string id = "") : base(id)
        {
            Items = new ControlCollection<MenuItem>(this);
        }
    }
}
