using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RayTracerHandler
    {

        /*
            Resource
            https://raytracing.github.io/


        */

        PixelDrawer pixelDrawer = new PixelDrawer();
        CameraRT camera;

        public void InitRayTracer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            camera = new CameraRT(graphics.GraphicsDevice.Viewport.AspectRatio, 2, 1, VectorD3.Zero());

            for (int y = pixelDrawer.yTotal-1; y >= 0; y--) 
            {
                for (int x = 0; x < pixelDrawer.xTotal; x++) 
                {
                    double u = (double)x / (pixelDrawer.xTotal-1);
                    double v = (double)y / (pixelDrawer.yTotal-1);

                    Ray r = new Ray(camera.origin, camera.lowerLeft + camera.horizontal * u + camera.vertical * v - camera.origin);

                    pixelDrawer.colors[pixelDrawer.GetIndexOnPos(new Vector2(x, (pixelDrawer.yTotal - 1) - y))] = getRayColor(r);
                }
            }
        }

        public void Draw(ref Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(ref pixel, spriteBatch, graphics);
        }

        Color getRayColor(Ray ray)
        {
            VectorD3 unitDir = VectorD3.Normalize(ray.direction);
            float t = (float)(0.5f * (unitDir.x + 1));
            return (1-t) * new Color(1.0f + (t * 0.5f), 1.0f + (t * 0.7f), 1.0f + (t * 1));
        }
    }
}