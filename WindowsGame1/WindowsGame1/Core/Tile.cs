using System;
using System.Linq;
using System.Runtime.InteropServices;
using WindowsGame1.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Core
{
    public class Tile
    {
        public Rectangle TileBounds;
        public float Friction = 0.80f;
        public Texture2D Texture;
        public int[,] Terrain;
        public Texture2D Bumpmap;

        public Tile(Texture2D texture)
        {
            Texture = texture;
            TileBounds = new Rectangle(0, 0, texture.Width, texture.Height);
            Terrain = new int[TileBounds.Width, TileBounds.Height];
        }

        public void UpdateBumpmap()
        {
            var colors1D = new Color[Texture.Width * Texture.Height];
            Game1.Textures[UnitsEnum.WATER1].GetData(colors1D);

            var bumpmapColors = new Color[Bumpmap.Width * Bumpmap.Height];
            Game1.Textures[UnitsEnum.WATERBUMP1].GetData(bumpmapColors);

            for (int x = 0; x < Texture.Width; x++)
            {
                for (int y = 0; y < Texture.Height; y++)
                {
                    var pos = x + y*Bumpmap.Width;
                    var pixel = Convert.ToByte(bumpmapColors[pos].R / 3);
                    colors1D[pos].R += pixel;
                    colors1D[pos].B += pixel;
                    colors1D[pos].G += pixel;
                }
            }

            Texture.SetData(colors1D);
        }

        //public Tile(Vector2 position, int width, int height)
        //{
        //    var widthRadius = width/2;
        //    var heightRadius = height/2;
        //    TileBounds = new Rectangle((int) position.X - widthRadius, 
        //                               (int) position.Y - heightRadius, 
        //                               width, 
        //                               height);
        //}
    }
}
