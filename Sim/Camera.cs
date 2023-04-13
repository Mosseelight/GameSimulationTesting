using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }

        public Camera(Vector2 position, float zoom = 1f)
        {
            Position = position;
            Zoom = zoom;
        }

        public Matrix GetViewMatrix(GraphicsDeviceManager graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateScale(Zoom, Zoom, 1) * Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth * 0.5f, graphics.PreferredBackBufferHeight * 0.5f, 0));
        }
    }
}