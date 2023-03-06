using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class VectorFieldSim
{
        
    Vector2[] vectorsDirs;
    Vector2[] vectorsPos;
    Object obj = new Object();
    int vectorFieldXLen = 10;
    int vectorFieldYLen = 10;
    int posCount;
    float distanceCheck = 1.1f;

    public void CreateVectorField()
    {
        vectorsDirs = new Vector2[vectorFieldXLen * vectorFieldYLen];
        vectorsPos = new Vector2[vectorFieldXLen * vectorFieldYLen];
        for (int y = 0; y < vectorFieldYLen; y++)
        {
            for (int x = 0; x < vectorFieldXLen; x++)
            {
                vectorsPos[posCount] = new Vector2(x,y);
                vectorsDirs[posCount] = new Vector2(CalculateXValue(x),CalculateYValue(y));
                Console.WriteLine(vectorsDirs[posCount]);
                posCount++;
            }
        }
        obj.startDir = new Vector2(1,1);
        obj.start();
    }

    public void ApplyFieldDirection()
    {
        for (int i = 0; i < vectorsPos.Length; i++)
        {
            if(Vector2.Distance(vectorsPos[i], obj.pos) < distanceCheck)
            {
                obj.UpdateDir(vectorsDirs[i] * 0.1f);
                Console.WriteLine(obj.curDir + " objdir");
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



        return input;
    }

    public float CalculateYValue(float input)
    {

        

        return input;
    }
}


