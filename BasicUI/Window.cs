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

        /// <summary>
        /// Locates a control with the given ID within the heirarchy
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="id">The id of the control to look for</param>
        /// <returns>The located control</returns>
        /// <remarks>Backed by a dictionary, or a recursive search if not found.</remarks>
        public T FindControlWithId<T>(string id) where T : Control
        {
            return RootContainer.FindControlWithId<T>(id);
        }

        /// <summary>
        /// Get a control by its ID
        /// </summary>
        /// <param name="controlId">The ID of the control to return</param>
        /// <returns>The control with the given ID</returns>
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

        /// <summary>
        /// Call this method to handle the rendering & processing loop yourself
        /// </summary>
        public void RenderLoop()
        {
            if (_nativeWindow == null)
            {
                InitializeWindow();
            }

            _nativeWindow.Visible = true;
            while (_nativeWindow.Visible)
            {
                WindowRenderer.RenderFrame(_nativeWindow, _graphicsContext, RootContainer);
                _nativeWindow.ProcessEvents();

                Thread.Sleep(16); //60fps
            }
        }

        /// <summary>
        /// Begin rendering the window in a separate thread
        /// </summary>
        public void StartRenderThread()
        {
            _renderThread = new Thread(RenderLoop);       
            _renderThread.Start();
        }

        /// <summary>
        /// Hide the window, and stop the render thread if running
        /// </summary>
        public void EndRendering()
        {
            _nativeWindow.Visible = false;
        }
    }


}
