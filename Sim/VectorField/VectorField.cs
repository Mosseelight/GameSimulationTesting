using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class VectorFieldSim
{
        
    Vector2[] vectors;
    int vectorFieldXLen = 10;
    int vectorFieldYLen = 10;
    int posCount;

    public void CreateVectorField()
    {
        vectors = new Vector2[vectorFieldXLen * vectorFieldYLen];
        for (int y = 0; y < vectorFieldYLen; y++)
        {
            for (int x = 0; x < vectorFieldXLen; x++)
            {
                vectors[posCount] = new Vector2(x,y);
                Console.WriteLine(vectors[posCount]);
                posCount++;
            }
        }
    }
}


