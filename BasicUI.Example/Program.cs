using System;
using BasicUI;
using System.Threading.Tasks;
using System.Threading;
using BasicUI.Controls;
using System.Text;
using System.Numerics;
using System.Net.Http;

namespace BasicUI.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Window w = new Window
            {
                FontPath = "Fonts/DroidSans.ttf",
                WindowTitle = "BasicUI.Example"
            };

            w.RootContainer.Add(new Frame
            {
                Title = "Hey Dudes",
                BackgroundAlpha = 0.75f,
                Size = new Vector2(256, 300),
                Children =
                {
                    new Button
                    {
                        Text = "Click Me!",
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
                    new BulletedList<string>("IPList")
                }
            });

            w.StartRenderThread();

            Console.WriteLine("Window Created! Push a key to stop.");
            Console.ReadLine();

            w.EndRendering();
        }
    }
}