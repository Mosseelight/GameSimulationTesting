using System;
using System.Collections.Generic;
using System.Linq;
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
        public Vector3 normal;
        public Color color = Color.White;
        int currentIndice = 0;

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verTop;
            vertices[2] = _verRight;
        }

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop, Color col)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verRight;
            vertices[2] = _verTop;
            color = col;
        }

        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop, Color col, Vector3 nor)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verTop;
            vertices[2] = _verRight;
            color = col;
            normal = nor;
        }
        public Triangle(Vertex _verLeft, Vertex _verRight, Vertex _verTop, Color col, Vector3 nor, int curInd)
        {
            vertices[0] = _verLeft;
            vertices[1] = _verTop;
            vertices[2] = _verRight;
            color = col;
            normal = nor;
            currentIndice = curInd;
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
            box.maxY = MathF.Max(vertices[0].position.Y, MathF.Min(vertices[1].position.Y, vertices[2].position.Y));
            return box;
        }

        public BoundBox TriangleBoundsProj()
        {
            BoundBox box = new BoundBox();
            box.minX = MathF.Min(vertices[0].scrPos.X, MathF.Min(vertices[1].scrPos.X, vertices[2].scrPos.X));
            box.minY = MathF.Min(vertices[0].scrPos.Y, MathF.Min(vertices[1].scrPos.Y, vertices[2].scrPos.Y));
            box.maxX = MathF.Max(vertices[0].scrPos.X, MathF.Min(vertices[1].scrPos.X, vertices[2].scrPos.X));
            box.maxY = MathF.Max(vertices[0].scrPos.Y, MathF.Min(vertices[1].scrPos.Y, vertices[2].scrPos.Y));
            return box;
        }

        public void FixWindingOrder()
        {
            Vector3 vertex0 = vertices[0].position;
            Vector3 vertex1 = vertices[1].position;
            Vector3 vertex2 = vertices[2].position;

            Vector3 e1 = vertices[1].position - vertices[0].position;
            Vector3 e2 = vertices[2].position - vertices[0].position;

            Vector3 cross = Vector3.Cross(e1, e2);

            if(cross.Z < 0)
            {
                Console.WriteLine(vertices[1].position);
                vertices[2].position = vertex1;
                vertices[1].position = vertex2;
                Console.WriteLine(vertices[1].position);
            }
        }

        public bool BackfaceCull(Vector3 cameraPos)
        {
            Vector3 camDist = TriangleCenter() - cameraPos;
            float dist = Vector3.Dot(camDist, normal);


            if(dist < 0)
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
            //front
            cube.tris[ind[0]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, -1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, 1.0f) + pos), Color.Green, new Vector3(-1.0f, -0, -0), ind[0]);
            cube.tris[ind[1]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, -1.0f) + pos), Color.Green, new Vector3(-1.0f, -0, -0), ind[1]);

            cube.tris[ind[2]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, -1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, -1.0f) + pos), Color.Red, new Vector3(-0, -0, -1.0f), ind[2]);
            cube.tris[ind[3]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, -1.0f) + pos), Color.Red, new Vector3(-0, -0, -1.0f), ind[3]);

            cube.tris[ind[4]] = new Triangle(new Vertex(new Vector3(1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, 1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, -1.0f) + pos), Color.Pink, new Vector3(1.0f, -0, -0), ind[4]);
            cube.tris[ind[5]] = new Triangle(new Vertex(new Vector3(1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, 1.0f) + pos), Color.Pink, new Vector3(1.0f, -0, -0), ind[5]);

            cube.tris[ind[6]] = new Triangle(new Vertex(new Vector3(1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, 1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, 1.0f) + pos), Color.Aqua, new Vector3(-0, -0, 1.0f), ind[6]);
            cube.tris[ind[7]] = new Triangle(new Vertex(new Vector3(1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, 1.0f) + pos), Color.Aqua, new Vector3(-0, -0, 1.0f), ind[7]);

            cube.tris[ind[8]] = new Triangle(new Vertex(new Vector3(1.0f, -1.0f, -1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, -1.0f) + pos), Color.Purple, new Vector3(-0, -1.0f, -0), ind[8]);
            cube.tris[ind[9]] = new Triangle(new Vertex(new Vector3(1.0f, -1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, -1.0f, 1.0f) + pos), new Vertex(new Vector3(-1.0f, -1.0f, 1.0f) + pos), Color.Purple, new Vector3(-0, -1.0f, -0), ind[9]);

            cube.tris[ind[10]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(1.0f, 1.0f, -1.0f) + pos), Color.Black, new Vector3(-0, 1.0f, -0), ind[10]);
            cube.tris[ind[11]] = new Triangle(new Vertex(new Vector3(-1.0f, 1.0f, -1.0f) + pos), new Vertex(new Vector3(-1.0f, 1.0f, 1.0f) + pos), new Vertex(new Vector3(1.0f, 1.0f, 1.0f) + pos), Color.Black, new Vector3(-0, 1.0f, -0), ind[11]);
            return cube;
        }

        public void SortIndices(Vector3 camPos)
        {
            if(tris.Length == 0)
                throw new Exception("No triangles");
            int[] indicesSorted = new int[tris.Length];
            List<double> distList = new List<double>();
            Vector3[] distsVec = new Vector3[tris.Length];
            double[] dists = new double[tris.Length];
            for (int v = 0; v < tris.Length; v++)
            {
                distsVec[v] = tris[v].TriangleCenter() - camPos;
                dists[v] = distsVec[v].Length();
                distList.Add(dists[v]);
            }
            distList.Sort();
            for (int v = 0; v < tris.Length; v++)
            {
               int index = Array.IndexOf(dists, distList[v]);
               indicesSorted[v] = index;
            }

            Triangle[] sortedTris = new Triangle[tris.Length];
            for (int v = 0; v < tris.Length; v++)
            {
                sortedTris[v] = tris[indices[v]];
            }

            tris = sortedTris;
        }
    }
}
