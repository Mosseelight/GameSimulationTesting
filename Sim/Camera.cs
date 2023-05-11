using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 LookAt {get; set;}
        public float Zoom { get; set; }

        public Camera(Vector3 position, Vector3 _lookAt, float zoom)
        {
            Position = position;
            LookAt = _lookAt;
            Zoom = zoom;
        }

        public Matrix GetViewMatrix(GraphicsDeviceManager graphics)
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, -Position.Z)) * Matrix.CreateScale(Zoom, Zoom, 1) * Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth * 0.5f, graphics.PreferredBackBufferHeight * 0.5f, 0));
        }
    }
}