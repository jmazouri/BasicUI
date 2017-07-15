using MvvmCross.Core.Views;
using System;
using MvvmCross.Core.ViewModels;
using BasicUI.Controls;

namespace BasicUI.MvvmCross
{
    public class BasicViewPresenter : MvxViewPresenter
    {
        private Window _window;

        public BasicViewPresenter(Window window)
        {
            _window = window;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        public override void Close(IMvxViewModel toClose)
        {
            throw new NotImplementedException();
        }

        public override void Show(MvxViewModelRequest request)
        {
            if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                _window.RootContainer = new Container
                {
                    BindingContext = instanceRequest.ViewModelInstance,
                    Children =
                    {
                        new Frame("mvxRoot")
                        {
                            
                        }
                    }
                };
            }
        }
    }
}
