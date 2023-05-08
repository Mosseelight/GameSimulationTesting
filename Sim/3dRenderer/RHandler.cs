using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RayTracerHandler
    {

        //To do things
        /*
        
        1. vertex shader
        get the projection done

        2. rasterization
        switch to z-buffer

        3. fragment shading
        idk??

        4. output
        under 20ms render time


        https://en.wikipedia.org/wiki/Graphics_pipeline

        https://en.wikipedia.org/wiki/Z-buffering
        Switch to this for drawing the pixels

        opencl for gpu acceleration

        */


        PixelDrawer pixelDrawer = new PixelDrawer();

        Triangle triangle;

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            triangle = new Triangle(new Vertex(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(50 * pixelDrawer.visualScale, 100 * pixelDrawer.visualScale, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(100 * pixelDrawer.visualScale, 50 * pixelDrawer.visualScale, 0), new Vector3(1, 1, 1), Color.Black));
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            triangle.verLeft.position.X++;
            triangle.verRight.position.X++;
            triangle.verTop.position.X++;
            triangle.verLeft.position.Y++;
            triangle.verRight.position.Y++;
            triangle.verTop.position.Y++;
            Rasterization();
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
            for (int i = 0; i < pixelDrawer.colors.Length; i++)
            {
                pixelDrawer.colors[i] = Color.White;
            }
        }

        void VertexShader()
        {
            
        }

        void Rasterization()
        {
            for (int x = 0; x < pixelDrawer.xTotal; x++)
            {
                for (int y = 0; y < pixelDrawer.yTotal; y++)
                {
                    Vector2 pos = pixelDrawer.GetPosOnIndex(y + pixelDrawer.yTotal * x);
                    if(triangle.ContainsPoint(pos))
                    {
                        pixelDrawer.colors[y + pixelDrawer.yTotal * x] = Color.Black;
                    }
                }
            }
        }




    }
}