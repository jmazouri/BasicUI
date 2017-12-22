using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public interface IMenuItem : IControl
    {
        string Label { get; set; }
        bool Enabled { get; set; }
        bool Selected { get; set; }
    }
}
