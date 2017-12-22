using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public interface IControl
    {
        /// <summary>
        /// The unique identifier for the control. Some controls require this to be unique in order to render properly.
        /// </summary>
        string Id { get; }
        object BindingContext { get; set; }

        void Render();
    }
}
