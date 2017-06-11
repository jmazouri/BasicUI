using BasicUI.Controls;
using System;
using System.Threading;
using ImGuiNET;
using System.Numerics;
using ImageSharp;
using ImageSharp.PixelFormats;

namespace BasicUI.FileBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Window w = new Window(1280, 720, "Basic Browser");
            
            w.RootContainer.Add(new Frame
            {
                BackgroundAlpha = 0.66f,
                Size = new Vector2(1024, 600),
                StartCentered = true,
                Children =
                {
                    new TextBox
                    {
                        Width = 1008,
                        InputTextFlags = InputTextFlags.EnterReturnsTrue
                    },
                    new Panel
                    {
                        Size = new Vector2(1008, 540),
                        Children =
                        {
                            new Button
                            {
                                Text = "Hey there!"
                            },
                            new ImageBox
                            {
                                Image = Image.Load<Argb32>("test.png"),
                                Size = new Vector2(256, 256)
                            },
                            new Repeater<string>
                            {
                                Renderer = item => ImGui.BulletText(item),
                                Items =
                                {
                                    "what", "the", "shit"
                                }
                            }
                        }
                    }
                }
            });

            CancellationTokenSource src = new CancellationTokenSource();
            w.StartRenderAsync(src);

            Console.ReadLine();
            src.Cancel();
        }
    }
}