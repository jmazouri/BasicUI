using System.Numerics;

namespace BasicUI
{
    public struct Color
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public Color(int red, int green, int blue, int alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public Color(int red, int green, int blue)
        {
            R = red;
            G = green;
            B = blue;
            A = 255;
        }

        public static implicit operator Vector4(Color c)
        {
            return new Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        public static implicit operator OpenTK.Color(Color c)
        {
            return new OpenTK.Color(c.R, c.G, c.B, c.A);
        }

        public static Color Red => new Color(255, 0, 0, 255);
        public static Color Green => new Color(0, 255, 0, 255);
        public static Color Blue => new Color(0, 0, 255, 255);
        public static Color Black => new Color(0, 0, 0, 255);
        public static Color White => new Color(255, 255, 255, 255);
        public static Color Transparent => new Color(0, 0, 0, 0);
    }
}
