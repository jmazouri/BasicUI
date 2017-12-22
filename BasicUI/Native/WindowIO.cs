using ImGuiNET;
using OpenTK;
using OpenTK.Input;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BasicUI.Native
{
    public static class WindowIO
    {
        private static float _wheelPosition;

        public static void SetKeyMappings(NativeWindow nativeWindow)
        {
            IO io = ImGui.GetIO();
            io.KeyMap[GuiKey.Tab] = (int)Key.Tab;
            io.KeyMap[GuiKey.LeftArrow] = (int)Key.Left;
            io.KeyMap[GuiKey.RightArrow] = (int)Key.Right;
            io.KeyMap[GuiKey.UpArrow] = (int)Key.Up;
            io.KeyMap[GuiKey.DownArrow] = (int)Key.Down;
            io.KeyMap[GuiKey.PageUp] = (int)Key.PageUp;
            io.KeyMap[GuiKey.PageDown] = (int)Key.PageDown;
            io.KeyMap[GuiKey.Home] = (int)Key.Home;
            io.KeyMap[GuiKey.End] = (int)Key.End;
            io.KeyMap[GuiKey.Delete] = (int)Key.Delete;
            io.KeyMap[GuiKey.Backspace] = (int)Key.BackSpace;
            io.KeyMap[GuiKey.Enter] = (int)Key.Enter;
            io.KeyMap[GuiKey.Escape] = (int)Key.Escape;
            io.KeyMap[GuiKey.A] = (int)Key.A;
            io.KeyMap[GuiKey.C] = (int)Key.C;
            io.KeyMap[GuiKey.V] = (int)Key.V;
            io.KeyMap[GuiKey.X] = (int)Key.X;
            io.KeyMap[GuiKey.Y] = (int)Key.Y;
            io.KeyMap[GuiKey.Z] = (int)Key.Z;

            nativeWindow.KeyDown += (sender, e) =>
            {
                ImGui.GetIO().KeysDown[(int)e.Key] = true;
                UpdateModifiers(e);
            };

            nativeWindow.KeyUp += (sender, e) =>
            {
                ImGui.GetIO().KeysDown[(int)e.Key] = false;
                UpdateModifiers(e);
            };

            nativeWindow.KeyPress += (sender, e) =>
            {
                ImGui.AddInputCharacter(e.KeyChar);
            };
        }

        private static unsafe void UpdateModifiers(KeyboardKeyEventArgs e)
        {
            IO io = ImGui.GetIO();
            io.AltPressed = e.Alt;
            io.CtrlPressed = e.Control;
            io.ShiftPressed = e.Shift;
        }

        public static void UpdateImGuiInput(NativeWindow nativeWindow, IO io)
        {
            MouseState cursorState = Mouse.GetCursorState();
            MouseState mouseState = Mouse.GetState();

            if (nativeWindow.Bounds.Contains(cursorState.X, cursorState.Y))
            {
                Point windowPoint = nativeWindow.PointToClient(new Point(cursorState.X, cursorState.Y));
                io.MousePosition = new System.Numerics.Vector2(windowPoint.X / io.DisplayFramebufferScale.X, windowPoint.Y / io.DisplayFramebufferScale.Y);
            }
            else
            {
                io.MousePosition = new System.Numerics.Vector2(-1f, -1f);
            }

            io.MouseDown[0] = mouseState.LeftButton == ButtonState.Pressed;
            io.MouseDown[1] = mouseState.RightButton == ButtonState.Pressed;
            io.MouseDown[2] = mouseState.MiddleButton == ButtonState.Pressed;

            float newWheelPos = mouseState.WheelPrecise;
            float delta = newWheelPos - _wheelPosition;
            _wheelPosition = newWheelPos;
            io.MouseWheel = delta;
        }
    }
}
