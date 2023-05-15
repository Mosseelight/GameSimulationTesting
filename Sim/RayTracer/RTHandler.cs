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

            camera = new CameraRT(graphics.GraphicsDevice.Viewport.AspectRatio, 2, 1, Vector3.Zero);

            for (int y = pixelDrawer.yTotal-1; y >= 0; y--) 
            {
                for (int x = 0; x < pixelDrawer.xTotal; x++) 
                {
                    double r = (double)x / (pixelDrawer.xTotal-1);
                    double g = (double)y / (pixelDrawer.yTotal-1);
                    double b = 0.25;

                    int ir = (int)(255.999 * r);
                    int ig = (int)(255.999 * g);
                    int ib = (int)(255.999 * b);

                    pixelDrawer.colors[pixelDrawer.GetIndexOnPos(new Vector2(x, (pixelDrawer.yTotal - 1) - y))] = new Color(ir, ig, ib);
                }
            }
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }

        Color getRayColor(ray ray)
        {
            Vector3 unitDir = Vector3.Normalize(ray.direction);
            float t = 0.5f * (unitDir.Y + 1);
            return (1-t) * new Color(1.0f + (t * 0.5f), 1.0f + (t * 0.7f), 1.0f + (t * 1));
        }
    }
}