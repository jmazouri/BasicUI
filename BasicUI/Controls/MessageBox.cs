using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BasicUI.Controls
{
    public class MessageBox : ModalPrompt, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Message
        {
            get => messageBox.Content;
            set => messageBox.Content = value;
        }

        public IEnumerable<string> Options { get; private set; }
        
        public Action<MessageBox, string> OptionSelected { get; set; }

        private Text messageBox;

        public MessageBox(string id = "msgBox", string message = null, params string[] options) : base(id)
        {
            Options = options;

            messageBox = new Text { Content = message };
            Message = message;

            var buttonLayout = new HorizontalLayout
            {
                BindingContext = BindingContext
            };

            if (Options != null)
            {
                buttonLayout.AddRange(options.Select(opt =>
                {
                    return new Button
                    {
                        Text = opt,
                        OnClick = button =>
                        {
                            OptionSelected.Invoke(this, opt);
                            IsOpen = false;

                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOpen)));
                        }
                    };
                }));
            }

            Add(messageBox);
            Add(buttonLayout);
        }
    }
}
