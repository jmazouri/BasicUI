using ImGuiNET;
using System;

namespace BasicUI.Controls
{
    public class ExceptionBox : ModalPrompt
    {
        private Text _boxContent = new Text("exText");

        private Exception _exception;
        public Exception Exception
        {
            get => _exception;
            set
            {
                _exception = value;

                _boxContent.Content = _exception.Message;
            }
        }

        public void Show(Exception ex)
        {
            Exception = ex;
            IsOpen = true;
        }

        public ExceptionBox(string id = "exBox") : base(id)
        {
            var buttonLayout = new HorizontalLayout
            {
                BindingContext = BindingContext,
                Children =
                {
                    new Button
                    {
                        Text = "Okay...",
                        OnClick = button =>
                        {
                            IsOpen = false;
                        }
                    }
                }
            };

            Add(_boxContent);
            Add(buttonLayout);

            PostRender += _ =>
            {
                if (ImGui.IsKeyPressed(ImGui.GetKeyIndex(GuiKey.Enter)))
                {
                    IsOpen = false;
                }
            };
        }
    }
}
