using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Core
{
    class Map
    {
        public Tile[,] Tiles;
        public float Friction;
        public Vector2 Wind;

        public Map(int xTiles, int yTiles)
        {
            Tiles = new Tile[xTiles,yTiles];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    var t = Tiles[x, y];
                    spriteBatch.Draw(t.Texture, new Vector2(x * t.Texture.Width, y * t.Texture.Height),
                                    t.TileBounds, Color.White, 0, new Vector2(0, 0),
                                    1, SpriteEffects.None, 0.9f);
                }
            }
        }
    }
}
