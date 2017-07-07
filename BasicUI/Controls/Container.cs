using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BasicUI.Controls
{
    public class Container : Control, IList<Control>
    {
        public Vector2 Size { get; set; }
        public ControlCollection<Control> Children { get; private set; }

        protected override void InternalRender()
        {
            foreach (var child in Children)
            {
                child.Render();
            }
        }

        private Control RecursiveControlSearch(string id, IEnumerable<Control> target)
        {
            foreach (IEnumerable<Control> c in target.Where(x => x is IEnumerable<Control>))
            {
                var recursiveResult = RecursiveControlSearch(id, c);
                if (recursiveResult?.Id == id)
                {
                    return recursiveResult;
                }
            }

            return target.FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// Locates a control with the given ID within the heirarchy
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="id">The id of the control to look for</param>
        /// <returns>The located control</returns>
        /// <remarks>Backed by a dictionary, or a recursive search if not found.</remarks>
        public T FindControlWithId<T>(string id) where T : Control
        {
            if (String.IsNullOrWhiteSpace(id)) throw new ArgumentOutOfRangeException("id", id, "Id must not be null or empty");

            if (Window.ControlIdentifiers.ContainsKey(id))
            {
                return (T)Window.ControlIdentifiers[id];
            }

            T found = (T)RecursiveControlSearch(id, this);

            if (found != null)
            {
                return found;
            }
            else throw new KeyNotFoundException($"Control not found with id \"{id}\"");
        }

        public Container(string id = "") : base(id)
        {
            Children = new ControlCollection<Control>(this);
        }

        public virtual void Add(Control item) => Children.Add(item);
        public void AddRange(IEnumerable<Control> items) => Children.AddRange(items);
        public void Insert(int index, Control item) => Children.Insert(index, item);
        public Control this[int index] { get => Children[index]; set => Children[index] = value; }
        public int Count => Children.Count;
        public bool IsReadOnly => false;
        public void Clear() => Children.Clear();
        public bool Contains(Control item) => Children.Contains(item);
        public void CopyTo(Control[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public IEnumerator<Control> GetEnumerator() => Children.GetEnumerator();
        public int IndexOf(Control item) => Children.IndexOf(item);
        public bool Remove(Control item) => Children.Remove(item);
        public void RemoveAt(int index) => Children.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
    }
}
