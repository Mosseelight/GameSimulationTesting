using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public struct Vertex
    {
        public Vector3 position;
        public Vector2 scrPos;
        public Vector3 normal;
        public Color color;

        public Vertex(Vector3 pos, Vector3 nor, Color col)
        {
            position = pos;
            normal = nor;
            color = col;
            scrPos = Vector2.Zero;
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
}