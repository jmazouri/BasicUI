using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using System;
using System.Threading.Tasks;

namespace BasicUI.MvvmCross.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var app = new App();
            
            app.Run().GetAwaiter().GetResult();
        }
    }

    public class App : MvxApplication
    {
        private Window _window;

        public App()
        {
            _window = new Window();

            MvxSimpleIoCContainer.Initialize();

            Mvx.RegisterType<ICalculation, Calculation>();
            Mvx.RegisterSingleton<IMvxViewPresenter>(new BasicViewPresenter(_window));
            RegisterAppStart(new ExampleAppStart());
        }

        public async Task Run()
        {
            //Initialize();
            await Mvx.Resolve<Window>().StartRenderAsync();

            await Task.Delay(-1);
        }
    }
}
