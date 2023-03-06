using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

public class VectorFieldSim
{
        
    Vector2[] vectorsDirs;
    Vector2[] vectorsPos;
    Object obj = new Object();
    int vectorFieldXLen = 10;
    int vectorFieldYLen = 10;
    int vectorFieldScale = 10;
    int posCount;

    public void CreateVectorField()
    {
        vectorsDirs = new Vector2[vectorFieldXLen * vectorFieldYLen];
        vectorsPos = new Vector2[vectorFieldXLen * vectorFieldYLen];
        for (int y = 0; y < vectorFieldYLen; y++)
        {
            for (int x = 0; x < vectorFieldXLen; x++)
            {
                vectorsPos[posCount] = new Vector2(x * vectorFieldScale,y * vectorFieldScale);
                vectorsDirs[posCount] = new Vector2(CalculateXValue(x),CalculateYValue(y));
                posCount++;
            }
        }
        obj.pos = new Vector2(850, 500);
    }

    public void ApplyFieldDirection()
    {

        List<float> distances = new List<float>();
        for (int i = 0; i < vectorsPos.Length; i++)
        {
            distances.Add(Vector2.Distance(vectorsPos[i], obj.pos));
            if(distances.Min() == distances.Min() && distances.Count == vectorsPos.Length)
            {
                Console.WriteLine(Vector2.Distance(vectorsPos[i], obj.pos));
                obj.UpdateDir(vectorsDirs[i] * 0.1f);
                distances.Clear();
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

    public float CalculateXValue(float input)
    {

        //input *= 1;

        return input;
    }

    public float CalculateYValue(float input)
    {

        //input *= 1;
        return input;
    }
}


