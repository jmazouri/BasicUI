using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public abstract class EditableControl : ControlBase
    {
        public abstract string Value { get; }

        public EditableControl(string id = "") : base(id)
        {
        }
    }
}
