using MvvmCross.Core.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace BasicUI.MvvmCross
{
    public class MvxBasicSetup : MvxSetup
    {
        protected override IMvxApplication CreateApp()
        {
            throw new NotImplementedException();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            throw new NotImplementedException();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            throw new NotImplementedException();
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            throw new NotImplementedException();
        }

        protected override IMvxViewsContainer CreateViewsContainer()
        {
            throw new NotImplementedException();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            throw new NotImplementedException();
        }
    }
}
