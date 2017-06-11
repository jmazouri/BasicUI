using ImGuiNET;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.Controls
{
    public class TextBox : Control
    {
        public float Width { get; set; }
        public string Label { get; set; } = "##data";
        public InputTextFlags InputTextFlags { get; set; }

        /// <summary>
        /// The action to perform when text has changed. Set InputTextFlags to EnterReturnsTrue to only call when enter is pressed.
        /// </summary>
        public Action<string> OnEdit { get; set; }

        public string Text { get; private set; }

        private byte[] _textBuffer;

        protected override unsafe void InternalRender()
        {
            ImGui.PushItemWidth(Width);

            if (ImGui.InputText(Label, _textBuffer, (uint)_textBuffer.Length, InputTextFlags, data => 0))
            {
                Text = Encoding.UTF8.GetString(_textBuffer);
                OnEdit(Text);
            }

            ImGui.PopItemWidth();
        }

        /// <summary>
        /// Initializes a new TextBox control
        /// </summary>
        /// <param name="id">The ID of the textbox</param>
        /// <param name="bufferSize">The maximum size of the string the textbox can return, in bytes.</param>
        public TextBox(string id = "", int bufferSize = 1024) : base(id)
        {
            _textBuffer = new byte[bufferSize];
        }
    }
}
