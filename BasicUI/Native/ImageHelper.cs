using ImageSharp;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using ImageSharp.PixelFormats;

namespace BasicUI.Native
{
    public static class ImageHelper
    {
        static Dictionary<string, IntPtr> _textureIds = new Dictionary<string, IntPtr>();
        public static IEnumerable<string> LoadedImages => _textureIds.Keys;

        /// <summary>
        /// Loads a texture, or returns the pointer to an existing texture
        /// </summary>
        /// <param name="image">The image to load</param>
        /// <returns>A pointer referring to the loaded OpenGL texture</returns>
        public static IntPtr LoadImage(string textureName, Image<Argb32> image = null)
        {
            if (!_textureIds.ContainsKey(textureName))
            {   
                if (String.IsNullOrWhiteSpace(textureName) || image == null)
                {
                    throw new ArgumentNullException("image", "Image to load was null, and textureName was invalid or not found in cache.");
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
                _textureIds.Add(textureName, new IntPtr(texture));
            }

            //Return a pointer to the texture ID
            return _textureIds[textureName];
        }

        public static void UnloadImage(string textureName)
        {
            if (_textureIds.ContainsKey(textureName))
            {
                GL.DeleteTexture(_textureIds[textureName].ToInt32());
                _textureIds.Remove(textureName);
            }
        }
    }
}
