using ImageSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using ImageSharp.PixelFormats;

namespace BasicUI.Native
{
    public static class ImageHelper
    {
        static Dictionary<string, int> _textureIds = new Dictionary<string, int>();

        /// <summary>
        /// Loads a texture, or returns the pointer to an existing texture
        /// </summary>
        /// <param name="image">The image to load</param>
        /// <returns>A pointer referring to the loaded OpenGL texture</returns>
        public static IntPtr LoadImage(string textureName, Image<Argb32> image)
        {
            if (_textureIds.ContainsKey(textureName))
            {
                return new IntPtr(_textureIds[textureName]);
            }

            //Create a new texture ID
            var texture = GL.GenTexture();

            //Start setting up a 2D texture
            GL.BindTexture(TextureTarget.Texture2D, texture);

            //Get the bytes out of our image
            byte[] packed = image.Pixels.AsBytes().ToArray();

            //Allocate some unmanaged memory, and fill it
            IntPtr targetPtr = Marshal.AllocHGlobal(packed.Length);
            Marshal.Copy(packed, 0, targetPtr, packed.Length);

            //Fill the GPU's memory
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, targetPtr);

            //Set some texture parameters otherwise everything is white
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

            //Free the memory we allocated earlier
            Marshal.FreeHGlobal(targetPtr);

            //Keep track of the texture
            _textureIds.Add(textureName, texture);
            
            //Return a pointer to the texture ID
            return new IntPtr(texture);
        }

        public static void UnloadImage(string textureName)
        {
            if (_textureIds.ContainsKey(textureName))
            {
                GL.DeleteTexture(_textureIds[textureName]);
                _textureIds.Remove(textureName);
            }
        }
    }
}
