using ImGuiNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BasicUI.Controls
{
    public class ComboBox : EditableControl, INotifyPropertyChanged
    {
        public string Label { get; set; } = String.Empty;
        public List<string> Items { get; set; } = new List<string>();
        public Func<IEnumerable<string>> ItemSelector { get; set; }

        public IEnumerable<string> ReadableItems => ItemSelector != null ? ItemSelector() : Items;

        private int _currentItem;
        public string CurrentItem => ReadableItems.ElementAt(_currentItem);

        public override string Value => CurrentItem;

        protected override void InternalRender()
        {
            ImGui.Combo(Label, ref _currentItem, ReadableItems.ToArray());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ComboBox(string id = "") : base(id) { }
    }
}
