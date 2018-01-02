using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public interface IMenuItem : IControl
    {
        string Label { get; }
        bool Enabled { get; }
        bool Selected { get; }
    }
}
