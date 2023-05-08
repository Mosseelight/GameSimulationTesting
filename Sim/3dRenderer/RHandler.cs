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

        Triangle[] triangle = new Triangle[2];

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            camera = new Camera(new Vector2(pixelDrawer.xOffset * pixelDrawer.visualScale, pixelDrawer.yOffset * pixelDrawer.visualScale), 1);

            triangle[0] = new Triangle(new Vertex(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(0, 500, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(500, 500, 0), new Vector3(1, 1, 1), Color.Black));
            triangle[1] = new Triangle(new Vertex(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(500, 0, 0), new Vector3(1, 1, 1), Color.Black), new Vertex(new Vector3(500, 500, 0), new Vector3(1, 1, 1), Color.Black));
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Rasterization();
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(graphics));
            for (int x = 0; x < pixelDrawer.xTotal; x++)
            {
                for (int y = 0; y < pixelDrawer.yTotal; y++) 
                {
                    spriteBatch.Draw(pixel, new Vector2(x * pixelDrawer.visualScale, y * pixelDrawer.visualScale), null, pixelDrawer.colors[y + pixelDrawer.yTotal * x], 0, Vector2.Zero, new Vector2(pixelDrawer.visualScale, pixelDrawer.visualScale), SpriteEffects.None, 0f);
                }
            }
            spriteBatch.End();
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
            for (int i = 0; i < triangle.Length; i++)
            {
                for (int x = 0; x < pixelDrawer.xTotal; x++)
                {
                    for (int y = 0; y < pixelDrawer.yTotal; y++)
                    {
                        Vector2 pos = pixelDrawer.GetPosOnIndex(y + pixelDrawer.yTotal * x);
                        if(triangle[i].ContainsPoint(pos))
                        {
                            if(i == 0)
                            {
                                pixelDrawer.colors[y + pixelDrawer.yTotal * x] = Color.Red;
                            }
                            else
                            {
                                pixelDrawer.colors[y + pixelDrawer.yTotal * x] = Color.Green;
                            }
                        }
                    }
                }
            }
        }




    }
}