using MvvmCross.Core.Views;
using System;
using MvvmCross.Core.ViewModels;
using BasicUI.Controls;

namespace BasicUI.MvvmCross
{
    public class BasicViewPresenter : IMvxViewPresenter
    {
        private Window _window;

        public BasicViewPresenter(Window window)
        {
            _window = window;
        }

        public void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint
        {
            throw new NotImplementedException();
        }

        public void ChangePresentation(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        public void Close(IMvxViewModel toClose)
        {
            throw new NotImplementedException();
        }

        public void Show(MvxViewModelRequest request)
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
