using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RayTracerHandler
    {

        //To do things
        /*
        
        1. vertex shader
        2. rasterization
        3. fragment shading
        4. output

        https://en.wikipedia.org/wiki/Graphics_pipeline

        opencl for gpu acceleration

        */


        PixelDrawer pixelDrawer = new PixelDrawer();

        Triangle triangle;

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 1;
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