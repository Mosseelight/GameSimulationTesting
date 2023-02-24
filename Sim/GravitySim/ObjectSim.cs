using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;

public class ObjSimulation : Game
{

    int numObjs = 3;
    Object[] objects;


    float lossCollideAmount = 0.01f;
    public void StRunSimulation()
    {
        objects = new Object[numObjs];
        //give objects settings
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = new Object();
            objects[i].mass = new Random().Next(6,12);
            //780. 460
            int randomX = new Random().Next(0,780);
            int randomY = new Random().Next(0,460);
            objects[i].pos = new Vector2(randomX, randomY);
            int randomXS = new Random().Next(0,10);
            int randomYS = new Random().Next(0,10);
            objects[i].startDir = new Vector2(randomXS, randomYS);
        }
    }

    public void UpRunSimulation()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
            objects[i].UpdateDir(acceleration);
            objects[i].UpdatePos();
            objects[i].UpdateColl(acceleration);
        }
    }

    public void DrawSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(circle, objects[i].pos, Color.White);
            spriteBatch.End();
        }
    }

    public Vector2 CalculateAcceleration(Vector2 point, Object ignore = null)
    {
        
        Vector2 acceleration = Vector2.Zero;
        for (int i = 0; i < objects.Length; i++)
        {
            //very important if statement to check if the object is the same as its self
            //beacuse then that messes up the dist cal as you cannot divide 0 the dist
            //to itself is always 0
            if (objects[i] != ignore) {
                float dst = Vector2.Distance(objects[i].pos, point); //(float)Math.Sqrt(Vector2.Dot(objects[i].pos, point));
                Vector2 forceDir = Vector2.Normalize(objects[i].pos - point);
                acceleration += forceDir * 7f * objects[i].mass / dst;
            }
            Console.WriteLine(acceleration);
            //loss of speed check
            if(objects[i].pos.X > 780 || objects[i].pos.X < 0 || objects[i].pos.Y > 460 || objects[i].pos.Y < 0)
            {
                acceleration *= lossCollideAmount;
            }
        }

        return acceleration;
    }

}
