using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public abstract class Repeater<T> : Control, ICollection<T>
    {
        protected List<T> Items { get; set; } = new List<T>();
        public Func<T, string> Selector { get; set; }

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
            RenderItems();

            if (_scrollNextRender && Items.Count > 0)
            {
                ImGui.SetScrollHere();
                _scrollNextRender = false;
            }
        }

        public abstract void RenderItems();

        #region Interface Members
        public void Add(T item)
        {
            Items.Add(item);
            if (ScrollToBottom)
            {
                _scrollNextRender = true;
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            Items.AddRange(items);
            if (ScrollToBottom)
            {
                _scrollNextRender = true;
            }
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
