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
        public Vertex ver1;
        public Vertex ver2;
        public Vertex ver3;

        public Triangle(Vertex _ver1, Vertex _ver2, Vertex _ver3)
        {
            ver1 = _ver1;
            ver2 = _ver2;
            ver3 = _ver3;
        }

        public bool ContainsPoint(Vector2 point)
        {
            Vector2 p1 = new Vector2(ver1.position.X, ver1.position.Y);
            Vector2 p2 = new Vector2(ver2.position.X, ver2.position.Y);
            Vector2 p3 = new Vector2(ver3.position.X, ver3.position.Y);

            Vector2 x = p3 - p1;
            Vector2 y = p2 - p1;
            Vector2 b = point - p1;

            float xx = Vector2.Dot(x, x);
            float yx = Vector2.Dot(y, x);
            float xb = Vector2.Dot(x, b);
            float yy = Vector2.Dot(y, y);
            float yb = Vector2.Dot(y, b);

            float denom = xx*yy - yx*yx;
            float u = (yy * xb - yx * yb) / denom;
            float v = (xx * yb - yx * xb) / denom;

            return (u >= 0) && (v >= 0) && (u + v < 1);
        }
    }
}