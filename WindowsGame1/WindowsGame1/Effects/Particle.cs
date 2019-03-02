using System;
using WindowsGame1.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Effects
{
    public class Particle
    {
        private readonly Texture2D _texture;
        private Vector2 _position;
        private readonly Vector2 _velocity;
        private float _size;
        private float _fade = 1;
        private readonly float _rotation;
        private uint t;

        public bool Expired;
        public float GrowRate = 1.01f;
        public float FadeRate = 0.97f;

        public Particle(Vector2 position, Vector2 velocity, Texture2D texture,
                        float size)
        {
            var rnd = new Random();
            _position = position;
            _texture = texture;
            _velocity = rnd.Next(1, 2) * velocity;
            _size = rnd.Next(1, 2) * size;
            _rotation = (float) rnd.Next(0, 314)/100;
        }

        public void Update()
        {
            _position += _velocity;
            _size *= GrowRate;
            _fade *= FadeRate;

            if (_fade < 0.01f)
                Expired = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White * _fade, _rotation,
                             _texture.Origin(), _size, SpriteEffects.None, 0.2f);
        }
    }
}
