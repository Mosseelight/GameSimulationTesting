using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RayTracerHandler
    {
        PixelDrawer pixelDrawer = new PixelDrawer();

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 5;
            pixelDrawer.InitDrawer(graphics);
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}