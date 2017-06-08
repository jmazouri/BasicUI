using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BasicUI.Controls
{
    public class Container : Control, ICollection<Control>
    {
        public Vector2 Size { get; set; }
        public IList<Control> Children { get; set; } = new List<Control>();

        private static Dictionary<string, Control> controlIdentifiers = new Dictionary<string, Control>();

        public override void Render()
        {
            foreach (var child in Children)
            {
                child.Render();
            }
        }

        private Control RecursiveControlSearch(string id, Container target)
        {
            foreach (Container c in target.Children.Where(x => x is Container))
            {
                var recursiveResult = RecursiveControlSearch(id, c);
                if (recursiveResult?.Id == id)
                {
                    return recursiveResult;
                }
            }

            return target.Children.FirstOrDefault(d => d.Id == id);
        }

        public T FindControlWithId<T>(string id) where T : Control
        {
            if (String.IsNullOrWhiteSpace(id)) throw new ArgumentOutOfRangeException("id", id, "Id must not be null or empty");

            if (controlIdentifiers.ContainsKey(id))
            {
                return (T)controlIdentifiers[id];
            }

            T found = (T)RecursiveControlSearch(id, this);

            if (found != null)
            {
                return found;
            }
            else throw new KeyNotFoundException($"Control not found with id \"{id}\"");
        }

        #region Interface Members

        public int Count => Children.Count;
        public bool IsReadOnly => Children.IsReadOnly;
        
        public void Add(Control item)
        {
            if (!String.IsNullOrWhiteSpace(item.Id))
            {
                controlIdentifiers.Add(item.Id, item);
            }

            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(Control item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        public bool Remove(Control item)
        {
            return Children.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
        #endregion

        public Container(string id = "") : base(id) { }
    }
}
