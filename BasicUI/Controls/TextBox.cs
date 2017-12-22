using ImGuiNET;
using System.ComponentModel;
using System.Text;
using System;

namespace BasicUI.Controls
{
    public class TextBox : EditableControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float Width { get; set; }
        public string Label { get; set; } = "##data";
        public InputTextFlags InputTextFlags { get; set; }

        private bool _focus;
        public void Focus()
        {
            _focus = true;
        }

        public string Text
        {
            get
            {
                string decoded = Encoding.UTF8.GetString(_textBuffer);
                int charLocation = decoded.IndexOf('\0');

                return charLocation > 0 ? decoded.Substring(0, charLocation) : "";
            }
            set
            {
                if (value != null)
                {
                    _textBuffer = new byte[_textBuffer.Length];
                    Encoding.UTF8.GetBytes(value).CopyTo(_textBuffer, 0);

                    BoxEdited();
                }
            }
        }

        public override string Value => Text;

        private byte[] _textBuffer;

        protected override unsafe void InternalRender()
        {
            ImGui.PushItemWidth(Width);

            if (_focus)
            {
                ImGui.SetKeyboardFocusHere();

                _focus = false;
            }

            if (ImGui.InputText(Label, _textBuffer, (uint)_textBuffer.Length, InputTextFlags, data => 0))
            {
                BoxEdited();
            }

            ImGui.PopItemWidth();
        }

        private void BoxEdited()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
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
