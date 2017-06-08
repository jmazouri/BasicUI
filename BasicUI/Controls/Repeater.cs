using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicUI.Controls
{
    public abstract class Repeater<T> : Control, ICollection<T>
    {
        private List<T> Items { get; set; } = new List<T>();
        public Func<T, string> Selector { get; set; }
        public Color Color { get; set; } = Color.White;

        public bool ScrollToBottom { get; set; }
        private bool _scrollNextRender = false;

        public int Count => Items.Count;
        public bool IsReadOnly => false;

        public string GetItemValue(T item)
        {
            if (Selector != null)
            {
                return Selector(item);
            }
            else
            {
                return item.ToString();
            }
        }

        public override void Render()
        {
            ImGui.PushStyleColor(ColorTarget.Text, Color);

            //Pass in a copy to help avoid concurrency issues
            RenderItems(Items.ToList());

            ImGui.PopStyleColor();

            if (_scrollNextRender && Items.Count > 0)
            {
                ImGui.SetScrollHere(1);
                _scrollNextRender = false;
            }
        }

        public abstract void RenderItems(IEnumerable<T> items);

        #region Interface Members
        public void Add(T item)
        {
            if (ScrollToBottom)
            {
                _scrollNextRender = true;
            }

            Items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (ScrollToBottom)
            {
                _scrollNextRender = true;
            }

            Items.AddRange(items);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        #endregion

        public Repeater(string id = "", Func<T, string> selector = null) : base(id)
        {
            Selector = selector;
        }
    }
}
