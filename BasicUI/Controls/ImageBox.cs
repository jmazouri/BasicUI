using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using System.Numerics;
using ImageSharp;
using BasicUI.Native;
using ImageSharp.PixelFormats;
using System.IO;

namespace BasicUI.Controls
{
    public class ImageBox : Control
    {
        public Color TintColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Black;

        public Vector2 Size { get; set; }

        public Image<Argb32> Image { get; set; } = new Image<Argb32>(1, 1);

        protected override void InternalRender()
        {
            Vector2 renderSize = Size == default(Vector2) ? new Vector2(Image.Width, Image.Height) : Size;

            ImGui.Image(ImageHelper.LoadImage(Id, Image), renderSize, Vector2.Zero, Vector2.One, TintColor, BorderColor);
        }

        public ImageBox(string id = "") : base(id) { }
    }
}
