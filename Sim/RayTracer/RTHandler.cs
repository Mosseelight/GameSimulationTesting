using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RayTracerHandler
    {

        PixelDrawer pixelDrawer = new PixelDrawer();

        public void InitRayTracer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 1;
            pixelDrawer.InitDrawer(graphics);
            for (int x = 0; x < pixelDrawer.xTotal; x++)
            {
                for (int y = 0; y < pixelDrawer.yTotal; y++)
                {
                    Ray ray = new Ray();
                    ray.origin = Vector3.Zero;
                    ray.direction = new Vector3(x,y,0);
                    pixelDrawer.colors[y + pixelDrawer.yTotal * x] = new Color(ray.direction.X, ray.direction.Y, ray.direction.Z);
                }
            }
        }


        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}