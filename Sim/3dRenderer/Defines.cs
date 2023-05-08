using System;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public struct Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Color color;

        public Vertex(Vector3 pos, Vector3 nor, Color col)
        {
            position = pos;
            normal = nor;
            color = col;
        }
    }

    public class Triangle
    {
        public Vertex verLeft; //left point
        public Vertex verRight; // right point
        public Vertex verTop; // top point

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop)
        {
            verLeft = _verLeft;
            verRight = _verRight;
            verTop = _verTop;
        }

        public bool ContainsPoint(Vector2 point)
        {
            Vector2 p1 = new Vector2(verLeft.position.X, verLeft.position.Y);
            Vector2 p2 = new Vector2(verTop.position.X, verTop.position.Y);
            Vector2 p3 = new Vector2(verRight.position.X, verRight.position.Y);

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