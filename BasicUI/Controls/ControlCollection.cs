using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BasicUI.Controls
{
    public class ControlCollection<T> : IList<T> where T : Control
    {
        private List<T> _children = new List<T>();
        private Control _parent;

        public ControlCollection(Control parent)
        {
            _parent = parent;
        }

        public void Add(T item)
        {
            PerformAddOperations(item);
            _children.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Insert(int index, T item)
        {
            PerformAddOperations(item);
            _children.Insert(index, item);
        }

        private void PerformAddOperations(T item)
        {
            if (!String.IsNullOrWhiteSpace(item.Id) && !Window.ControlIdentifiers.ContainsKey(item.Id))
            {
                Window.ControlIdentifiers.Add(item.Id, item);
            }

            if (_parent?.BindingContext != null)
            {
                item.BindingContext = _parent.BindingContext;

                var container = item as IEnumerable<Control>;
                if (container != null)
                {
                    RecursiveApplyBinding(container);
                }
            }
        }

        private void RecursiveApplyBinding(IEnumerable<Control> target)
        {
            foreach (IEnumerable<Control> c in target.Where(x => x is IEnumerable<Control>))
            {
                RecursiveApplyBinding(c);
            }

            foreach (Control c in target)
            {
                c.BindingContext = _parent.BindingContext;
            }
        }

        public T this[int index]
        {
            get => _children[index];
            set
            {
                PerformAddOperations(value);
                _children[index] = value;
            }
        }

        public int Count => _children.Count;
        public bool IsReadOnly => false;
        public void Clear() => _children.Clear();
        public bool Contains(T item) => _children.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _children.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _children.GetEnumerator();
        public int IndexOf(T item) => _children.IndexOf(item);
        public bool Remove(T item) => _children.Remove(item);
        public void RemoveAt(int index) => _children.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();
    }
}
