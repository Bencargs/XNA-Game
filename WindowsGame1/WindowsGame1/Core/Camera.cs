using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1.Core
{
    class Camera
    {
        public Rectangle Viewport;
        public Rectangle InnerViewport;

        public Camera(Vector2 position, int width, int height)
        {
            var widthRadius = width/2;
            var heightRadius = height/2;
            Viewport = new Rectangle((int) position.X - widthRadius,
                                     (int) position.Y - heightRadius,
                                           width, height);
            InnerViewport = new Rectangle(Viewport.X + 30,
                                          Viewport.Y + 30,
                                          Viewport.Width - 30,
                                          Viewport.Height - 160);
        }

        public void UpdateCamera(Rectangle viewport, Vector2 mouseState, KeyboardState keyState)
        {
            // Move the camera if the mouse is near the window bounds
            if (viewport.Contains((int)mouseState.X, (int)mouseState.Y))
            {
                if (mouseState.X < InnerViewport.Left || keyState.IsKeyDown(Keys.Left))
                    Viewport.X -= 10;
                else if (mouseState.X > InnerViewport.Width || keyState.IsKeyDown(Keys.Right))
                    Viewport.X += 10;
                if (mouseState.Y < InnerViewport.Top || keyState.IsKeyDown(Keys.Up))
                    Viewport.Y -= 10;
                else if (mouseState.Y > InnerViewport.Height || keyState.IsKeyDown(Keys.Down))
                    Viewport.Y += 10;
            }
        }

        public Matrix GetTransformations()
        {
            return Matrix.CreateTranslation(new Vector3(-Viewport.X, -Viewport.Y, 0));
        }
    }
}
