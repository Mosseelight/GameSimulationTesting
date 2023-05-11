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
        idk?? put sin on colors or something

        4. output
        under 20ms render time


        https://en.wikipedia.org/wiki/Graphics_pipeline

        https://en.wikipedia.org/wiki/Z-buffering
        Switch to this for drawing the pixels

        opencl for gpu acceleration

        put vertexs into a array of 3 in the triangle
        so can access in loops

        */


        PixelDrawer pixelDrawer = new PixelDrawer();
        Camera camera;
        Matrix projectionMat;
        Matrix viewMat;
        Matrix worldMat;

        Triangle[] triangle = new Triangle[4];

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            camera = new Camera(new Vector3(0, 0, -10), Vector3.Zero, 1);

            triangle[0] = new Triangle(new Vertex(new Vector3(0, 0, -10), Vector3.One, Color.Black), new Vertex(new Vector3(0, 10, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 10, -10), Vector3.One, Color.Black));
            triangle[1] = new Triangle(new Vertex(new Vector3(0, 0, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 0, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 10, -10), Vector3.One, Color.Black));

            triangle[2] = new Triangle(new Vertex(new Vector3(10, 0, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 0, -20), Vector3.One, Color.Black), new Vertex(new Vector3(10, 10, -20), Vector3.One, Color.Black));
            triangle[3] = new Triangle(new Vertex(new Vector3(10, 0, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 10, -10), Vector3.One, Color.Black), new Vertex(new Vector3(10, 10, -20), Vector3.One, Color.Black));
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            VertexShader(graphics);
            Rasterization();
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
            for (int i = 0; i < pixelDrawer.colors.Length; i++)
            {
                pixelDrawer.colors[i] = Color.White;
            }
        }

        void VertexShader(GraphicsDeviceManager graphics)
        {
            for (int i = 0; i < triangle.Length; i++)
            {
                for (int v = 0; v < 3; v++)
                {
                    
                }
            }
        }

        void Rasterization()
        {
            for (int i = 0; i < triangle.Length; i++)
            {
                for (int x = 0; x < pixelDrawer.xTotal; x++)
                {
                    for (int y = 0; y < pixelDrawer.yTotal; y++)
                    {
                        Vector2 pos = pixelDrawer.GetPosOnIndex(y + pixelDrawer.yTotal * x);
                        if(triangle[i].ContainsPoint(pos))
                        {
                            pixelDrawer.colors[y + pixelDrawer.yTotal * x] = Color.Green;
                        }
                    }
                }
            }
        }




    }
}