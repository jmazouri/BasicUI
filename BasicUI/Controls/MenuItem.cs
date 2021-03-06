﻿using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BasicUI.Controls
{
    public class MenuItem : ControlBase, IMenuItem, IEnumerable<IMenuItem>
    {
        protected ControlCollection<IMenuItem> Items { get; set; }

        public string Label { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Selected { get; set; }

        public Action<MenuItem> OnClick { get; set; }

        protected override void InternalRender()
        {
            if (Items.Any())
            {
                if (ImGui.BeginMenu(Label, Enabled))
                {
                    foreach (IMenuItem item in Items)
                    {
                        item.Render();
                    }

                    ImGui.EndMenu();
                }
            }
            else
            {
                if (ImGui.MenuItem(Label, "", Selected, Enabled))
                {
                    OnClick?.Invoke(this);
                }
            }
        }

        public void Add(IMenuItem item) => Items.Add(item);
        public IEnumerator<IMenuItem> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public MenuItem(string label, Action<MenuItem> onClick = null) : base(label)
        {
            Label = label;
            OnClick = onClick;

            Items = new ControlCollection<IMenuItem>(this);
        }

        public MenuItem(string label, string id) : base(id)
        {
            Label = label;
            Items = new ControlCollection<IMenuItem>(this);
        }
    }
}
