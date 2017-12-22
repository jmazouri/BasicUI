using System;
using BasicUI;
using System.Threading.Tasks;
using System.Threading;
using BasicUI.Controls;
using System.Text;
using System.Numerics;
using System.Net.Http;
using BasicUI.Native;
using ImageSharp;
using ImageSharp.PixelFormats;

namespace BasicUI.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Window w = new Window
            {
                //FontPath = "segoeui.ttf",
                WindowTitle = "BasicUI.Example"
            };

            w.RootContainer.Add(new Frame
            {
                Title = "Hey Dudes",
                BackgroundAlpha = 0.75f,
                Size = new Vector2(256, 300),
                StartCentered = true,
                Children =
                {
                    new Button
                    {
                        Text = "♨ what up",
                        OnClick = async (btn) =>
                        {
                            Text label = w["Status"] as Text;
                            var list = w.FindControlWithId<BulletedList<string>>("IPList");

                            label.Content = "Loading...";

                            HttpClient client = new HttpClient();
                            await Task.Delay(1500);
                            string ip = await client.GetStringAsync("http://checkip.amazonaws.com");

                            label.Content = "Done";
                            list.Add(ip);
                        }
                    },
                    new Text("Status")
                    {
                        Content = "Your IP Here",
                        Color = Color.Red
                    },
                    new NumericInput("fakeNumber")
                    {
                        Step = 1,
                        FastStep = 5,
                        Decimals = 2
                    },
                    new BulletedList<string>("IPList"),
                    new ImageBox()
                    {
                        Image = Image.Load<Argb32>("Images/test.jpg") 
                    }
                }
            });

            CancellationTokenSource src = new CancellationTokenSource();

            w.StartRenderAsync(src);

            Console.WriteLine("Window Created! Push a key to stop.");
            Console.ReadLine();

            src.Cancel();
        }
    }
}