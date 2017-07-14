using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI.MvvmCross.Example
{
    public class ExampleAppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<TipViewModel>();
        }
    }
}
