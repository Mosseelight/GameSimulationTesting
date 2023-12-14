using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace GameTesting {
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
            float minDist = float.MaxValue;
            int index = 0;
            Parallel.For (0, vectorsPos.Length, i =>
            {
                float dist = Vector2.Distance(vectorsPos[i], objects[b].pos);
                if(dist < minDist)
                {
                    minDist = dist;
                    index = i;
                }
            });
            objects[b].UpdateDir(Vector2.Normalize(vectorsDirs[index]) * speed);
            objects[b].UpdatePos();
            distances.Clear();
        });
    }


    //is needed because if the object goes out of the screen space and then it will still be calculating the distance to each vector
    //which I could fix using a quad tree, but it is easier to just to remove the object from the list
    public void CheckBounds()
    {
        //needed so the sim does not crash beacuse of index issues
        if(objects.Count != 1)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                //object is past left screen 
                if(objects[i].pos.X < 0)
                {
                    objects.Remove(objects[i]);
                }
                //object is past right screen 
                if(objects[i].pos.X > 1900)
                {
                    objects.Remove(objects[i]);
                }
                //object is above top screen 
                if(objects[i].pos.Y < 0)
                {
                    objects.Remove(objects[i]);
                }
                //object is below bottom screen
                if(objects[i].pos.Y > 1000)
                {
                    objects.Remove(objects[i]);
                }
            }
        }
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
        Vector2 textMiddlePos = font.MeasureString("Total: " + objects.Count) / 2;
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Total: " + objects.Count.ToString("N01"), new Vector2(textMiddlePos.X + 40, 1000), Color.Black, 0, textMiddlePos, 1.5f, SpriteEffects.None, 0.5f);
        for (int b = 0; b < objects.Count; b++)
        {
            spriteBatch.Draw(circle, objects[b].pos, Color.White);
            for (int i = 0; i < vectorsPos.Length; i++)
            {
                Vector2 origin = new Vector2(arrow.Width / 2f, arrow.Height / 2f);
                float angle = MathF.Atan2(vectorsDirs[i].Y, vectorsDirs[i].X);
                spriteBatch.Draw(arrow, vectorsPos[i], null, Color.White, angle, origin, Vector2.One, SpriteEffects.None, 0f);
                if(i == vectorsPos.Length - 1)
                {
                    spriteBatch.Draw(arrow, vectorsPos[i], null, Color.White, 0, origin, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }
        spriteBatch.End();
        CheckBounds();
    }

    public Vector2 CalculateVectorValue(float x, float y)
    {
        Vector2 result = new Vector2(-y,x);
        return result;
    }
}
}


