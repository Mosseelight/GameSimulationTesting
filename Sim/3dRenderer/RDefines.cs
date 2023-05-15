using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public struct Vertex
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 scrPos = Vector3.Zero;
        public Vector3 normal = Vector3.Zero;
        public Color color = Color.White;

        public Vertex(Vector3 pos, Vector3 nor, Color col)
        {
            position = pos;
            normal = nor;
            color = col;
        }

        public Vertex(Vector3 pos, Vector3 nor)
        {
            position = pos;
            normal = nor;
        }

        public Vertex(Vector3 pos)
        {
            position = pos;
        }
    }

    public struct BoundBox
    {
        public float minX = 0;
        public float minY = 0;
        public float maxX = 0;
        public float maxY = 0;

        public BoundBox(float _miX, float _miY, float _maX, float _maY)
        {
            minX = _miX;
            minY = _miY;
            maxX = _maX;
            maxY = _maY;
        }
    }

    public class Triangle
    {
        public Vertex[] vertices = new Vertex[3];
        public Color color = Color.White;

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verRight;
            vertices[2] = _verTop;
        }

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop, Color col)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verRight;
            vertices[2] = _verTop;
            color = col;
        }

        public bool ContainsPoint(Vector2 point)
        {
            Vector2 p1 = new Vector2(vertices[0].scrPos.X,vertices[0].scrPos.Y);
            Vector2 p2 = new Vector2(vertices[1].scrPos.X, vertices[1].scrPos.Y);
            Vector2 p3 = new Vector2(vertices[2].scrPos.X, vertices[2].scrPos.Y);

            Vector2 x = p3 - p1; 
            Vector2 y = p2 - p1;
            Vector2 b = point - p1; 

            float xx = Vector2.Dot(x, x);
            float yx = Vector2.Dot(y, x);
            float bx = Vector2.Dot(b, x);
            float yy = Vector2.Dot(y, y);
            float by = Vector2.Dot(b, y);

            float denom = xx*yy - yx*yx;
            float u = (yy*bx - yx*by) / denom;
            float v = (xx*by - yx*bx) / denom;

            return (u >= 0) && (v >= 0) && (u + v < 1);
        }

        public Vector3 TriangleCenter()
        {
            return (vertices[0].position + vertices[1].position + vertices[2].position)/3;
        }

        public Vector3 TriangleCenterProj()
        {
            return (vertices[0].scrPos + vertices[1].scrPos + vertices[2].scrPos)/3;
        }

        public BoundBox TriangleBounds()
        {
            BoundBox box = new BoundBox();
            box.minX = MathF.Min(vertices[0].position.X, MathF.Min(vertices[1].position.X, vertices[2].position.X));
            box.minY = MathF.Min(vertices[0].position.Y, MathF.Min(vertices[1].position.Y, vertices[2].position.Y));
            box.maxX = MathF.Max(vertices[0].position.X, MathF.Min(vertices[1].position.X, vertices[2].position.X));
            box.maxY = MathF.Max(vertices[0].position.X, MathF.Min(vertices[1].position.X, vertices[2].position.X));
            return box;
        }

        public BoundBox TriangleBoundsProj()
        {
            BoundBox box = new BoundBox();
            box.minX = MathF.Min(vertices[0].scrPos.X, MathF.Min(vertices[1].scrPos.X, vertices[2].scrPos.X));
            box.minY = MathF.Min(vertices[0].scrPos.Y, MathF.Min(vertices[1].scrPos.Y, vertices[2].scrPos.Y));
            box.maxX = MathF.Max(vertices[0].scrPos.X, MathF.Min(vertices[1].scrPos.X, vertices[2].scrPos.X));
            box.maxY = MathF.Max(vertices[0].scrPos.X, MathF.Min(vertices[1].scrPos.X, vertices[2].scrPos.X));
            return box;
        }

        public bool BackfaceCull(Vector3 cameraPos)
        {
            Vector3 u = vertices[1].position - vertices[0].position;
            Vector3 v = vertices[2].position - vertices[0].position;
            Vector3 camDist = TriangleCenter() - cameraPos;
            Vector3 triNorVec = Vector3.Cross(u,v);
            float d = -Vector3.Dot(triNorVec, vertices[0].position);
            float dist = Vector3.Dot(triNorVec, cameraPos) + d;
            

            if(dist >= 0)
                return true;

            return false;
        }
    }

    public class Mesh
    {
        public Triangle[] tris;
        public Vector3 position;
        public int[] indices;

        public Mesh CreateCube(Vector3 pos, int[] ind = null)
        {
            Mesh cube = new Mesh();
            cube.position = pos;
            cube.tris = new Triangle[12];
            cube.indices = new int[12];
            if(ind == null)
                ind = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            if(ind.Length != 12)
                throw new Exception("Indice length has to be 12");
            cube.indices = ind;
            cube.tris[ind[0]] = new Triangle(new Vertex(new Vector3(-1, -1, -1) + pos), new Vertex(new Vector3(-1, -1, 1) + pos), new Vertex(new Vector3(-1, 1, 1) + pos), Color.Green);
            cube.tris[ind[1]] = new Triangle(new Vertex(new Vector3(1, 1, -1) + pos), new Vertex(new Vector3(-1, -1, -1) + pos), new Vertex(new Vector3(-1, 0, -1) + pos), Color.Green);
            cube.tris[ind[2]] = new Triangle(new Vertex(new Vector3(1, -1, 1) + pos), new Vertex(new Vector3(-1, -1, -1) + pos), new Vertex(new Vector3(1, -1, -1) + pos), Color.Pink);
            cube.tris[ind[3]] = new Triangle(new Vertex(new Vector3(1, 1, -1) + pos), new Vertex(new Vector3(1, -1, -1) + pos), new Vertex(new Vector3(-1, -1, -1) + pos), Color.Pink);
            cube.tris[ind[4]] = new Triangle(new Vertex(new Vector3(-1, -1, -1) + pos), new Vertex(new Vector3(-1, 1, 1) + pos), new Vertex(new Vector3(-1, 1, -1) + pos), Color.Purple);
            cube.tris[ind[5]] = new Triangle(new Vertex(new Vector3(1, -1, 1) + pos), new Vertex(new Vector3(-1, -1, 1) + pos), new Vertex(new Vector3(-1, -1, -1) + pos), Color.Purple);
            cube.tris[ind[6]] = new Triangle(new Vertex(new Vector3(-1, 1, 1) + pos), new Vertex(new Vector3(-1, -1, 1) + pos), new Vertex(new Vector3(1, 1, 1) + pos), Color.Red);
            cube.tris[ind[7]] = new Triangle(new Vertex(new Vector3(1, 1, 1) + pos), new Vertex(new Vector3(1, -1, -1) + pos), new Vertex(new Vector3(1, 1, -1) + pos), Color.Red);
            cube.tris[ind[8]] = new Triangle(new Vertex(new Vector3(1, -1, -1) + pos), new Vertex(new Vector3(1, 1, 1) + pos), new Vertex(new Vector3(1, -1, 1) + pos), Color.Aqua);
            cube.tris[ind[9]] = new Triangle(new Vertex(new Vector3(1, 1, 1) + pos), new Vertex(new Vector3(1, 1, -1) + pos), new Vertex(new Vector3(-1, 1, -1) + pos), Color.Aqua);
            cube.tris[ind[10]] = new Triangle(new Vertex(new Vector3(1,1,1) + pos), new Vertex(new Vector3(-1, 1, -1) + pos), new Vertex(new Vector3(-1, 1, 1) + pos), Color.Black);
            cube.tris[ind[11]] = new Triangle(new Vertex(new Vector3(1,1,1) + pos), new Vertex(new Vector3(-1, 1, 1) + pos), new Vertex(new Vector3(1, -1, 1) + pos), Color.Black);
            return cube;
        }
    }
}