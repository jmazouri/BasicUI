using BasicUI.Controls;
using BasicUI.Native;

using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace BasicUI
{
    public class Window
    {
        private Thread _renderThread;
        private NativeWindow _nativeWindow;
        private GraphicsContext _graphicsContext;
        private int s_fontTexture;

        public Container RootContainer { get; private set; } = new Container();
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string FontPath { get; set; }
        public int FontSize { get; set; } = 14;

        private string initialTitle = "BasicUI";
        public string WindowTitle
        {
            get => _nativeWindow?.Title ?? initialTitle;
            set
            {
                if (_nativeWindow == null)
                {
                    initialTitle = value;
                }
                else
                {
                    _nativeWindow.Title = value;
                }
            }
        }

        public Window(int width = 640, int height = 480, string windowTitle = "BasicUI")
        {
            initialTitle = windowTitle;
            Width = width;
            Height = height;
        }

        public T FindControlWithId<T>(string id) where T : Control
        {
            return RootContainer.FindControlWithId<T>(id);
        }

        public Control this[string controlId] => RootContainer.FindControlWithId<Control>(controlId);

        private unsafe void InitializeWindow()
        {
            _nativeWindow = new NativeWindow(Width, Height, initialTitle, GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
            _graphicsContext = WindowRenderer.InitializeGraphicsContext(_nativeWindow);

            _nativeWindow.Visible = true;

            if (FontPath == null)
            {
                ImGui.GetIO().FontAtlas.AddDefaultFont();
            }
            else
            {
                var font = ImGui.GetIO().FontAtlas.AddFontFromFileTTF(FontPath, FontSize);
            }

            WindowIO.SetOpenTKKeyMappings();

            WindowRenderer.CreateDeviceObjects(ref s_fontTexture);
        }

        public void RenderLoop()
        {
            if (_nativeWindow == null)
            {
                InitializeWindow();
            }

            WindowLoop();
        }

        public void StartRenderThread()
        {
            _renderThread = new Thread(RenderLoop);       
            _renderThread.Start();
        }

        public void EndRenderThread()
        {
            _nativeWindow.Visible = false;
        }

        public void WindowLoop()
        {
            _nativeWindow.Visible = true;
            while (_nativeWindow.Visible)
            {
                WindowRenderer.RenderFrame(_nativeWindow, _graphicsContext, RootContainer);
                _nativeWindow.ProcessEvents();

                Thread.Sleep(16); //60fps
            }
        }
    }


}
