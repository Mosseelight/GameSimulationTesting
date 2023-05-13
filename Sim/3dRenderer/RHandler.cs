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
        backface culling
        order by z distnace

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
        Matrix perspectiveMat;
        float fov = 45;
        float nearValue = 0.01f;
        float farValue = 100f;

        Matrix viewMat;
        Matrix worldMat;
        Matrix projectMat;

        Triangle[] triangle = new Triangle[4];
        Mesh[] meshes = new Mesh[1];

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            camera = new Camera(new Vector3(0, -2, 20), Vector3.Zero, 1);
            perspectiveMat = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), pixelDrawer.xTotal / pixelDrawer.yTotal, nearValue, farValue);
            viewMat = Matrix.CreateLookAt(camera.Position, camera.LookAt, Vector3.Up);
            projectMat = perspectiveMat * viewMat;

            triangle[0] = new Triangle(new Vertex(new Vector3(0, 0, 5), Vector3.One, Color.Black), new Vertex(new Vector3(0, 1, 5), Vector3.One, Color.Black), new Vertex(new Vector3(1, 1, 5), Vector3.One, Color.Black));
            triangle[1] = new Triangle(new Vertex(new Vector3(0, 0, 5), Vector3.One, Color.Black), new Vertex(new Vector3(1, 0, 5), Vector3.One, Color.Black), new Vertex(new Vector3(1, 1, 5), Vector3.One, Color.Black));

            triangle[2] = new Triangle(new Vertex(new Vector3(0, 1, 5), Vector3.One, Color.Black), new Vertex(new Vector3(0, 1, 6), Vector3.One, Color.Black), new Vertex(new Vector3(1, 1, 6), Vector3.One, Color.Black));
            triangle[3] = new Triangle(new Vertex(new Vector3(0, 1, 5), Vector3.One, Color.Black), new Vertex(new Vector3(1, 1, 6), Vector3.One, Color.Black), new Vertex(new Vector3(1, 1, 6), Vector3.One, Color.Black));

            meshes[0] = new Mesh().CreateCube(new Vector3(2, 0, 3));
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
                    Vector4 vertexPos = new Vector4(triangle[i].vertices[v].position, 1);
                    vertexPos = Vector4.Transform(vertexPos, projectMat);
                    vertexPos = new Vector4(vertexPos.X / vertexPos.W, vertexPos.Y / vertexPos.W, vertexPos.Z / vertexPos.W, vertexPos.W);
                    triangle[i].vertices[v].scrPos = new Vector2(((vertexPos.X + 1)/2)*pixelDrawer.xTotal, (((vertexPos.Y * -1) + 1)/2)*pixelDrawer.yTotal);
                }
            }

            for (int m = 0; m < meshes.Length; m++)
            {
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    for (int vv = 0; vv < 3; vv++)
                    {
                        Vector4 vertexPos = new Vector4(meshes[m].tris[v].vertices[vv].position, 1);
                        vertexPos = Vector4.Transform(vertexPos, projectMat);
                        vertexPos = new Vector4(vertexPos.Y / vertexPos.W, vertexPos.X / vertexPos.W, vertexPos.Z / vertexPos.W, vertexPos.W);
                        meshes[m].tris[v].vertices[vv].scrPos = new Vector2(((vertexPos.X + 1)/2)*pixelDrawer.xTotal, (((vertexPos.Y * -1) + 1)/2)*pixelDrawer.yTotal);
                    }
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

            for (int m = 0; m < meshes.Length; m++)
            {
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    for (int x = 0; x < pixelDrawer.xTotal; x++)
                    {
                        for (int y = 0; y < pixelDrawer.yTotal; y++)
                        {
                            Vector2 pos = pixelDrawer.GetPosOnIndex(y + pixelDrawer.yTotal * x);
                            if(meshes[m].tris[v].ContainsPoint(pos))
                            {
                                pixelDrawer.colors[y + pixelDrawer.yTotal * x] = meshes[m].tris[v].color;
                            }
                        }
                    }
                }
            }
        }




    }
}