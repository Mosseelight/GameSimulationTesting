using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class VectorFieldSim
{
        
    Vector2[] vectorsDirs;
    Vector2[] vectorsPos;
    Vector2 screenMiddle = new Vector2(950,500);
    List<Object> objects = new List<Object>();
    bool spawnKeyPressed = false;
    //18, 18, 60 is a good number
    //if vectorlen changes keep the change to x = y and divide the scale if increse to len and multiply scale if decrese to len
    int vectorFieldXLen = 36;
    float vectorFieldXLenOffset = 400f;
    int vectorFieldYLen = 36;
    float vectorFieldYLenOffset = 0f;
    float vectorFieldScale = 30f;
    int vectorFieldXLenDir;
    int vectorFieldYLenDir;
    int posCount;
    int dirCount;
    float speed = 4;

    public void CreateVectorField()
    {
        if(vectorFieldXLen % 2 == 1)
        {
            vectorFieldXLen++;
        }
        if(vectorFieldYLen % 2 == 1)
        {
            vectorFieldYLen++;
        }
        vectorFieldXLenDir = vectorFieldXLen / 2;
        vectorFieldYLenDir = vectorFieldYLen / 2;
        vectorsDirs = new Vector2[vectorFieldXLen * vectorFieldYLen];
        vectorsPos = new Vector2[vectorFieldXLen * vectorFieldYLen];
        //Position for drawing the vector fields
        for (int y = 0; y < vectorFieldYLen; y++)
        {
            for (int x = 0; x < vectorFieldXLen; x++)
            {
                vectorsPos[posCount] = new Vector2(x * vectorFieldScale + vectorFieldXLenOffset, y * vectorFieldScale + vectorFieldYLenOffset);
                posCount++;
            }
        }
        //directions the vector field goes
        for (int y = -vectorFieldYLenDir; y < vectorFieldYLenDir; y++)
        {
            for (int x = -vectorFieldXLenDir; x < vectorFieldXLenDir; x++)
            {
                vectorsDirs[dirCount] = CalculateVectorValue(x,y);
                dirCount++;
            }
        }
        objects.Add(new Object());
        objects[0].pos = new Vector2(new Random().Next(0,1900), new Random().Next(0,1000));
    }

    public void ApplyFieldDirection()
    {
        Parallel.For (0, objects.Count, b =>
        {
            List<float> distances = new List<float>();
            float minDist;
            minDist = 0;
            for (int i = 0; i < vectorsPos.Length; i++)
            {
                distances.Add(Vector2.Distance(vectorsPos[i], objects[b].pos));
                if(distances.Count == vectorsPos.Length)
                {
                    minDist = distances.Min();
                }
            }
            int index = distances.IndexOf(minDist);
            objects[b].UpdateDir(Vector2.Normalize(vectorsDirs[index]) * speed);
            objects[b].UpdatePos();
            distances.Clear();
        });
    }


    public void HandleInput()
    {
        if(Keyboard.GetState().IsKeyDown(Keys.Q) && !spawnKeyPressed)
        {
            spawnKeyPressed = true;
            objects.Add(new Object());
            objects[objects.Count - 1].enabled = true;
            objects[objects.Count - 1].pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
        if(Keyboard.GetState().IsKeyUp(Keys.Q))
        {
            spawnKeyPressed = false;
        }
        if(Keyboard.GetState().IsKeyDown(Keys.W))
        {
            objects.Add(new Object());
            objects[objects.Count - 1].enabled = true;
            objects[objects.Count - 1].pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }

    public void DrawSim(Texture2D circle, Texture2D arrow, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
    {
        for (int b = 0; b < objects.Count; b++)
        {
            spriteBatch.Begin();
            for (int i = 0; i < vectorsPos.Length; i++)
            {
                Vector2 origin = new Vector2(arrow.Width / 2f, arrow.Height / 2f);
                float angle = (float)Math.Atan2(vectorsDirs[i].Y, vectorsDirs[i].X);
                spriteBatch.Draw(arrow, vectorsPos[i], null, Color.White, angle, origin, Vector2.One, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(circle, objects[b].pos, Color.White);
            spriteBatch.End();
        }
    }

    public Vector2 CalculateVectorValue(float x, float y)
    {
        Vector2 result = new Vector2(x-y * 500, y+x * 500);
        return result;
    }
}


