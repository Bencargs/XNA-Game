using System;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using WindowsGame1.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Common
{
    static class Extensions
    {
        /// <summary>
        /// Textures are usually drawn from their x=0, y=0 point
        /// This returns a point in the centre of an image
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Vector2 Origin(this Texture2D texture)
        {
            if (texture == null)
                return new Vector2(1,1);

            return new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
        }

        public static Vector2 Truncate(this Vector2 source, float max)
        {
            if (source.Length() > max)
            {
                source.Normalize();
                source *= max;
            }
            return source;
        }

        public static Vector2 Normalize(this Vector2 source, float length)
        {
            source.Normalize();
            source *= length;
            return source;
        }

        public static Vector2 Rotate(this Vector2 point, float radians)
        {
            var cosRadians = (float)Math.Cos(radians);
            var sinRadians = (float)Math.Sin(radians);

            return new Vector2(
                point.X * cosRadians - point.Y * sinRadians,
                point.X * sinRadians + point.Y * cosRadians);
        }

        public static float Angle(this Vector2 source)
        {
            return (float)(Math.Atan2(source.Y, source.X) + Math.PI / 2);
        }

        public static bool Collision(this UnitData source, Vector2 point)
        {
            var widthRadius = source.Texture.Width/2;
            var heightRadius = source.Texture.Height/2;
            var boundingBox = new Rectangle((int) source.Position.X - widthRadius, 
                                            (int) source.Position.Y - heightRadius, 
                                                  source.Texture.Width, 
                                                  source.Texture.Height);

            var collisionBox = new Rectangle((int) point.X, 
                                             (int) point.Y,
                                                   source.CollisionDistance,
                                                   source.CollisionDistance);

            return collisionBox.Intersects(boundingBox);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Vector2 source, Vector2 dest,
            Color colour, int width = 1)
        {
            var direction = dest - source;
            var angle = (float) Math.Atan2(direction.Y, direction.X);
            var distance = Vector2.Distance(source, dest);
            spriteBatch.Draw(texture, source, new Rectangle((int) source.X, (int) source.Y, (int) distance, 1), colour,
                angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
