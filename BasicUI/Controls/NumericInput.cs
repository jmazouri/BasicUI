using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class NumericInput : Control
    {
        private float _value;
        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public string Label { get; set; }

        public int Step { get; set; }
        public int FastStep { get; set; }
        public int Decimals { get; set; }

        public InputTextFlags InputTextFlags { get; set; }

        protected unsafe override void InternalRender()
        {
            fixed (float* ptr_value = &_value)
            {
                ImGuiNative.igInputFloat(Label, ptr_value, Step, FastStep, Decimals, InputTextFlags);
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
