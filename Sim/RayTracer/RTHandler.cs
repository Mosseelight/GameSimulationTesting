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
            PhyObject pObject = new PhyObject();
            pObject.radius = 100;
            pixelDrawer.visualScale = 5;
            pixelDrawer.InitDrawer(graphics);
            pObject.position = new Vector3(pixelDrawer.xOffset, pixelDrawer.yOffset, 0);
            for (int x = 0; x < pixelDrawer.xTotal; x++)
            {
                for (int y = 0; y < pixelDrawer.yTotal; y++)
                {
                    Ray ray = new Ray();
                    ray.origin = Vector3.Zero;
                    ray.direction = new Vector3(x,y,0);
                    if(hitObject(ray, pObject))
                    {
                        pixelDrawer.colors[y + pixelDrawer.yTotal * x] = new Color(255, 0, 0);
                    }
                    else
                    {
                        pixelDrawer.colors[y + pixelDrawer.yTotal * x] = new Color(255, 255, 255);
                    }
                }
            }
        }

        bool hitObject(Ray ray, PhyObject pOject)  
        {
            float calValue = ((ray.direction.X - pOject.position.X) * (ray.direction.X - pOject.position.X)) + ((ray.direction.Y - pOject.position.Y) * (ray.direction.Y - pOject.position.Y));
            if(calValue <= pOject.radius * pOject.radius)
            {
                return true;
            }
            return false;
        }


        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}