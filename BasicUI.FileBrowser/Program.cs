using BasicUI.Controls;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using OpenTK.Input;

namespace BasicUI.FileBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            BrowserViewModel viewModel = new BrowserViewModel();
            Binder<BrowserViewModel> binder = new Binder<BrowserViewModel>(viewModel);
            CancellationTokenSource src = new CancellationTokenSource();

            var driveMenu = new MenuItem("Drives");

            //TODO: See if this works crossplat
            foreach (var info in DriveInfo.GetDrives())
            {
                string path = info.RootDirectory.FullName;
                driveMenu.Add(new MenuItem(path, menu => menu.GetBinding<BrowserViewModel>().CurrentDirectory = path));
            }

            Window w = new Window(1280, 720, "Basic Browser")
            {
                BindingContext = viewModel,
                FontPath = "Fonts/Roboto-Regular.ttf",
                FontSize = 16,
                Toolbar =
                {
                    new MenuItem("File")
                    {
                        driveMenu,
                        new Separator(),
                        new MenuItem("Exit")
                        {
                            OnClick = menu => src.Cancel()
                        }
                    },
                    new MenuItem("View")
                    {
                        new EnumMenuItem<Sorting>("Sort", sort => viewModel.Sorting = sort),
                        new MenuItem("Animate Labels", _ => viewModel.AnimateLabels = !viewModel.AnimateLabels)
                    }
                }
            };

            //This fires when all the GL contexts and such have been set up,
            //so it's a good time to load textures 
            w.Ready += window =>
            {
                IconLoader.LoadAll("Icons");
                
                if (Directory.Exists(@"C:\"))
                {
                    viewModel.CurrentDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
                }
                else
                {
                    viewModel.CurrentDirectory = Environment.GetEnvironmentVariable("HOME");
                }

                window.HotkeyManager.Register(() => window.FindControlWithId<TextBox>("pathBox").Focus(), KeyModifiers.Control, Key.F);
            };

            BrowserWindow.Setup(w);
            BrowserWindow.CreateBindings(binder, w);

            w.StartRenderAsync(src).GetAwaiter().GetResult();
        }
    }
}
 