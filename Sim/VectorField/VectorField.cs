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
    Vector2 screenMiddle = new Vector2(950,500);
    Object obj = new Object();
    int vectorFieldXLen = 500;
    int vectorFieldYLen = 500;
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
                vectorsDirs[posCount] = CalculateVectorValue(new Vector2(x,y));
                posCount++;
            }
        }
        obj.pos = new Vector2(new Random().Next(0,1900), new Random().Next(0,1000));
    }

    public void ApplyFieldDirection()
    {

        List<float> distances = new List<float>();
        float minDist;
        minDist = 0;
        for (int i = 0; i < vectorsPos.Length; i++)
        {
            distances.Add(Vector2.Distance(vectorsPos[i], obj.pos));
            if(distances.Count == vectorsPos.Length)
            {
                minDist = distances.Min();
            }
        }
        int index = distances.IndexOf(minDist);
        obj.UpdateDir(Vector2.Normalize(vectorsDirs[index]));
        Console.WriteLine(obj.curDir);
        obj.UpdatePos();
        distances.Clear();
    }

    public void DrawSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(circle, obj.pos, Color.White);
        spriteBatch.End();
    }

    public Vector2 CalculateVectorValue(Vector2 pos)
    {
        pos *= -1;
        return pos;
    }
}


