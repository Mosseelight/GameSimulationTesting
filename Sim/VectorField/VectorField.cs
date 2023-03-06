using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class VectorFieldSim
{
        
    Vector2[] vectors;
    Object obj = new Object();
    int vectorFieldXLen = 10;
    int vectorFieldYLen = 10;
    int posCount;
    float distanceCheck = 1f;

    public void CreateVectorField()
    {
        vectors = new Vector2[vectorFieldXLen * vectorFieldYLen];
        for (int y = 0; y < vectorFieldYLen; y++)
        {
            for (int x = 0; x < vectorFieldXLen; x++)
            {
                vectors[posCount] = new Vector2(((1/x) * ));
                Console.WriteLine(vectors[posCount]);
                posCount++;
            }
        }
        obj.pos = new Vector2(1.2f,1.4f);
    }

    public void ApplyFieldDirection()
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            if(Vector2.Distance(vectors[i], obj.pos) < distanceCheck)
            {
                obj.UpdateDir(vectors[i]);
                Console.WriteLine(vectors[i]);
            }
        }
        obj.UpdatePos();
    }

    public void DrawSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(circle, obj.pos, Color.White);
            spriteBatch.End();
        }
}


