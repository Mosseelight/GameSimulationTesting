using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class RendererHandler
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
        float nearValue = 0.1f;
        float farValue = 100f;
        Matrix viewMat;
        Matrix worldMat;
        Matrix projectMat;

        float[] zDepth;

        Triangle[] triangle = new Triangle[4];
        Mesh[] meshes = new Mesh[1];

        public void InitRenderer(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);

            zDepth = new float[pixelDrawer.xTotal * pixelDrawer.yTotal];
            for (int x = 0; x < pixelDrawer.xTotal; x++)
            {
                for (int y = 0; y < pixelDrawer.yTotal; y++)
                {
                   zDepth[y + pixelDrawer.yTotal * x] = int.MaxValue; 
                }
            }

            camera = new Camera(new Vector3(0, 0, 10), Vector3.Zero, 1);
            meshes[0] = new Mesh().CreateCube(new Vector3(0, 0, 0));

            for (int m = 0; m < meshes.Length; m++)
            {
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    meshes[m].tris[v].FixWindingOrder();
                }
            }
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, float time)
        {
            worldMat = Matrix.Identity;
            perspectiveMat = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), pixelDrawer.xTotal / pixelDrawer.yTotal, nearValue, farValue);
            viewMat = Matrix.CreateLookAt(camera.Position -= new Vector3(0.1f, 0, 0.1f), camera.LookAt, Vector3.Up);
            projectMat = worldMat * viewMat * perspectiveMat;


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
            for (int m = 0; m < meshes.Length; m++)
            {
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    for (int vv = 0; vv < 3; vv++)
                    {
                        Vector4 vertexPos = new Vector4(meshes[m].tris[v].vertices[vv].position, 1);
                        vertexPos = Vector4.Transform(vertexPos, projectMat);
                        vertexPos = new Vector4(vertexPos.X / vertexPos.W, vertexPos.Y / vertexPos.W, vertexPos.Z / vertexPos.W, vertexPos.W);
                        meshes[m].tris[v].vertices[vv].scrPos = new Vector3(((vertexPos.X + 1)/2)*pixelDrawer.xTotal, (((vertexPos.Y * -1) + 1)/2)*pixelDrawer.yTotal, vertexPos.Z);
                    }
                }
            }
        }

        void Rasterization()
        {
            /*Parallel.For (0, meshes.Length, m =>
            {
                //meshes[m].SortIndices(camera.Position);
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    if(!meshes[m].tris[v].BackfaceCull(camera.Position))
                    {
                        for (int x = (int)meshes[m].tris[v].TriangleBoundsProj().minX; x < (int)meshes[m].tris[v].TriangleBoundsProj().maxX; x++)
                        {
                            for (int y = (int)meshes[m].tris[v].TriangleBoundsProj().minY; y < (int)meshes[m].tris[v].TriangleBoundsProj().maxY; y++)
                            {
                                Vector2 pos = new Vector2(x,y);
                                if(meshes[m].tris[v].ContainsPoint(pos))
                                {
                                    pixelDrawer.colors[pixelDrawer.GetIndexOnPos(pos)] = meshes[m].tris[v].color;
                                }
                            }
                        }
                    }
                }
            });*/

            Parallel.For (0, meshes.Length, m =>
            {
                for (int v = 0; v < meshes[m].tris.Length; v++)
                {
                    if(meshes[m].tris[v].BackfaceCull(camera.Position))
                    {
                        for (int x = 0; x < pixelDrawer.xTotal; x++)
                        {
                            for (int y = 0; y < pixelDrawer.yTotal; y++)
                            {
                                Vector2 pos = new Vector2(x,y);
                                if(meshes[m].tris[v].ContainsPoint(pos))
                                {
                                    pixelDrawer.colors[(pixelDrawer.yTotal * x) + y] = meshes[m].tris[v].color;
                                }
                            }
                        }
                    }
                }
            });
        }




    }
}