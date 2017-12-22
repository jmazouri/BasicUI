using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BasicUI.Controls
{
    public class Container : ControlBase, IList<IControl>
    {
        public Vector2 Size { get; set; }
        public ControlCollection<IControl> Children { get; private set; }

        protected override void InternalRender()
        {
            foreach (var child in Children.ToList())
            {
                child.Render();
            }
        }

        private IControl RecursiveControlSearch(string id, IEnumerable<IControl> target)
        {
            foreach (IEnumerable<IControl> c in target.Where(x => x is IEnumerable<IControl>))
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
        public T FindControlWithId<T>(string id) where T : IControl
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
            Children = new ControlCollection<IControl>(this);
        }

        public virtual void Add(IControl item) => Children.Add(item);
        public void AddRange(IEnumerable<IControl> items) => Children.AddRange(items);
        public void Insert(int index, IControl item) => Children.Insert(index, item);
        public IControl this[int index] { get => Children[index]; set => Children[index] = value; }
        public int Count => Children.Count;
        public bool IsReadOnly => false;
        public void Clear() => Children.Clear();
        public bool Contains(IControl item) => Children.Contains(item);
        public void CopyTo(IControl[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public IEnumerator<IControl> GetEnumerator() => Children.GetEnumerator();
        public int IndexOf(IControl item) => Children.IndexOf(item);
        public bool Remove(IControl item) => Children.Remove(item);
        public void RemoveAt(int index) => Children.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
    }
}
