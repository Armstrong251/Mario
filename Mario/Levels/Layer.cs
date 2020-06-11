
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Levels
{
    public class Layer
    {
        private Color[] data;
        private int height;
        private int width;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public Layer(Texture2D texture)
        {
            data = new Color[texture.Width * texture.Height];
            Width = texture.Width;
            Height = texture.Height;
            texture.GetData(data);
        }
        public Color this[int one, int two]
        {
            get => data[one + two * Width];
        }
    }
}