using BasicUI.Controls;
using BasicUI.Native;

using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

namespace BasicUI
{
    public class Window
    {
        private NativeWindow _nativeWindow;
        private GraphicsContext _graphicsContext;
        private int s_fontTexture;

        public Container RootContainer { get; private set; } = new Container();
        public object BindingContext { get => RootContainer.BindingContext; set => RootContainer.BindingContext = value; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public string FontPath { get; set; }
        public int FontSize { get; set; } = 14;

        public bool ShowToolbar { get; set; } = true;
        public Toolbar Toolbar { get; private set; }

        public HotkeyManager HotkeyManager { get; private set; }

        public static Dictionary<string, IControl> ControlIdentifiers = new Dictionary<string, IControl>();

        /// <summary>
        /// Runs after the window has been initialized, IO has been bound, and an OpenGL context has been created
        /// </summary>
        public Action<Window> Ready { get; set; }

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

        public static float GlobalTime = 0;

        public Window(int width = 640, int height = 480, string windowTitle = "BasicUI")
        {
            initialTitle = windowTitle;
            Width = width;
            Height = height;

            Toolbar = new Toolbar("mainToolbar");
            Toolbar.BindingContext = RootContainer.BindingContext;
        }

        /// <summary>
        /// Locates a control with the given ID within the heirarchy
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="id">The id of the control to look for</param>
        /// <returns>The located control</returns>
        /// <remarks>Backed by a dictionary, or a recursive search if not found.</remarks>
        public T FindControlWithId<T>(string id) where T : IControl
        {
            return RootContainer.FindControlWithId<T>(id);
        }

        /// <summary>
        /// Get a control by its ID
        /// </summary>
        /// <param name="controlId">The ID of the control to return</param>
        /// <returns>The control with the given ID</returns>
        public IControl this[string controlId] => RootContainer.FindControlWithId<IControl>(controlId);

        private unsafe void InitializeWindow()
        {
            _nativeWindow = new NativeWindow(Width, Height, initialTitle, GameWindowFlags.Default, GraphicsMode.Default, DisplayDevice.Default);
            _graphicsContext = WindowRenderer.InitializeGraphicsContext(_nativeWindow);

            _nativeWindow.Visible = true;

            //new ushort[] { '\u0001', '\uFFFF', 0 }
            FontHelper.LoadFont(FontHelper.LocateFont(FontPath), FontSize);

            WindowIO.SetKeyMappings(_nativeWindow);

            WindowRenderer.CreateDeviceObjects(ref s_fontTexture);

            if (File.Exists("theme.json"))
            {
                ThemeLoader.LoadTheme(File.ReadAllText("theme.json"));
            }

            HotkeyManager = new HotkeyManager(_nativeWindow);

            Ready?.Invoke(this);
        }

        /// <summary>
        /// Call this method to handle the rendering & processing loop yourself
        /// </summary>
        public void RenderLoop(CancellationTokenSource cancel = null)
        {
            if (_nativeWindow == null)
            {
                InitializeWindow();
            }

            _nativeWindow.Visible = true;
            while (_nativeWindow.Visible)
            {
                WindowRenderer.RenderFrame(_nativeWindow, _graphicsContext, this);
                _nativeWindow.ProcessEvents();

                Thread.Sleep(16); //60fps
                GlobalTime++;

                if ((cancel != null && cancel.IsCancellationRequested) || !_nativeWindow.Visible)
                {
                    GlobalTime = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// Begin rendering the window in a Task
        /// </summary>
        public Task StartRenderAsync(CancellationTokenSource src = null)
        {   
            return Task.Run(() => RenderLoop(src));
        }
    }


}
