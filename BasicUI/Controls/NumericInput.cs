using ImGuiNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BasicUI.Controls
{
    public class NumericInput : EditableControl, INotifyPropertyChanged
    {
        private float _value;

        public event PropertyChangedEventHandler PropertyChanged;

        public float SelectedValue
        {
            get => _value;
            set => _value = value;
        }

        public string Label { get; set; }

        public int Step { get; set; }
        public int FastStep { get; set; }
        public int Decimals { get; set; }

        public InputTextFlags InputTextFlags { get; set; }

        public override string Value => SelectedValue.ToString();

        protected unsafe override void InternalRender()
        {
            float oldValue = _value;
            ImGuiExtra.InputFloat(Label, ref _value, Step, FastStep, Decimals, InputTextFlags);
            
            if (_value != oldValue)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedValue)));
            }
        }

        public NumericInput(string id = "", string label = "") : base(id)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                Label = id;
            }
            else
            {
                Label = label;
            }
        }
    }
}
