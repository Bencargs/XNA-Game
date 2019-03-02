using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Common
{
    public class SolidColourTexture : Texture2D
    {
        private Color color;
        // Gets or sets the color used to create the texture
        public Color Color
        {
            get { return color; }
            set
            {
                if (value != color)
                {
                    color = value;
                    SetData(new[] { color });
                }
            }
        }


        public SolidColourTexture(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, 1, 1)
        {
            //default constructor
        }

        public SolidColourTexture(GraphicsDevice graphicsDevice, Color color)
            : base(graphicsDevice, 1, 1)
        {
            Color = color;
        }

    }
}
