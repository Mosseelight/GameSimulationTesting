using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public struct Vertex
    {
        public Vector3 position = Vector3.Zero;
        public Vector2 scrPos = Vector2.Zero;
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

    public class Triangle
    {
        public Vertex[] vertices = new Vertex[3];
        

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verRight;
            vertices[2] = _verTop;
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
            float v = v = (xx*by - yx*bx) / denom;

            return (u >= 0) && (v >= 0) && (u + v < 1);
        }
    }

    public class Mesh
    {
        public Triangle[] tris;
        public Vector3 position;

        public Mesh CreateCube(Vector3 pos)
        {
            Mesh cube = new Mesh();
            cube.position = pos;
            cube.tris = new Triangle[12];
            //front
            cube.tris[0] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(0, 1, 0) + pos), new Vertex(new Vector3(1, 1, 0) + pos));
            cube.tris[1] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(1, 0, 0) + pos), new Vertex(new Vector3(1, 1, 0) + pos));
            //top
            cube.tris[2] = new Triangle(new Vertex(new Vector3(0, 1, 0) + pos), new Vertex(new Vector3(0, 1, 1) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            cube.tris[3] = new Triangle(new Vertex(new Vector3(0, 1, 0) + pos), new Vertex(new Vector3(1, 1, 0) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            //right
            cube.tris[4] = new Triangle(new Vertex(new Vector3(1, 0, 0) + pos), new Vertex(new Vector3(1, 1, 0) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            cube.tris[5] = new Triangle(new Vertex(new Vector3(1, 0, 0) + pos), new Vertex(new Vector3(1, 0, 1) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            //left
            cube.tris[6] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(0, 0, 1) + pos), new Vertex(new Vector3(0, 1, 1) + pos));
            cube.tris[7] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(0, 1, 0) + pos), new Vertex(new Vector3(0, 1, 1) + pos));
            //back
            cube.tris[8] = new Triangle(new Vertex(new Vector3(0, 0, 1) + pos), new Vertex(new Vector3(0, 1, 1) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            cube.tris[9] = new Triangle(new Vertex(new Vector3(0, 0, 1) + pos), new Vertex(new Vector3(1, 0, 1) + pos), new Vertex(new Vector3(1, 1, 1) + pos));
            //bottom
            cube.tris[10] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(1, 0, 0) + pos), new Vertex(new Vector3(1, 0, 1) + pos));
            cube.tris[11] = new Triangle(new Vertex(Vector3.Zero + pos), new Vertex(new Vector3(0, 0, 1) + pos), new Vertex(new Vector3(1, 0, 1) + pos));
            return cube;
        }
    }
}