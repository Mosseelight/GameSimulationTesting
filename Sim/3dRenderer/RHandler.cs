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

        Triangle triangle = new Triangle(new Vertex(new Vector3(-1, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(1, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(0, 1, 0), new Vector3(1, 1, 1), Color.Black));

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 5;
            pixelDrawer.InitDrawer(graphics);
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }

        void VertexShader()
        {
            
        }




    }
}