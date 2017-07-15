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
            base.Initialize();
            MvxSimpleIoCContainer.Initialize();

            _window = new Window();
            var presenter = new BasicViewPresenter(_window);
            var start = new ExampleAppStart();

            Mvx.RegisterSingleton(_window);
            Mvx.RegisterType<ICalculation, Calculation>();
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
            RegisterAppStart(start);

            start.Start();
            

            //CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();
        }

        

        public async Task Run()
        {
            //Mvx.Resolve<IMvxAppStart>().Start();
            await Mvx.Resolve<Window>().StartRenderAsync();

            await Task.Delay(-1);
        }
    }
}
