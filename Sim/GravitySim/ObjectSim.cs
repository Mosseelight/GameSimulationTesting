using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;

public class ObjSimulation : Game
{

    int numObjs = 150;
    Object[] objects;


    bool haveSun = false;
    
    public void StRunSimulation()
    {
        objects = new Object[numObjs];
        //give objects settings
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = new Object();
            if(i == 0 && haveSun)
            {
                objects[i].mass = 20;
                int X = 900;
                int Y = 500;
                objects[i].pos = new Vector2(X, Y);
            }
            else
            {
                objects[i].mass = new Random().Next(6, 12);
                int randomX = new Random().Next(0, 1900);
                int randomY = new Random().Next(0, 1000);
                objects[i].pos = new Vector2(randomX, randomY);
            }
            //780. 460
            int randomXS = new Random().Next(10,40);
            int randomYS = new Random().Next(10,40);
            objects[i].startDir = new Vector2(randomXS, randomYS);
            objects[i].start();
        }
    }

    public void UpRunSimulation()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(i == 0 && haveSun)
            {
                Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
                objects[i].UpdateDir(acceleration);
                objects[i].UpdateColl(acceleration);
            }
            else
            {
                Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
                objects[i].UpdateDir(acceleration);
                objects[i].UpdatePos();
                objects[i].UpdateColl(acceleration);
            }

            for (int b = 0; b < objects.Length; b++)
            {
                if(b != i && Vector2.Distance(objects[i].pos, objects[b].pos) < 10)
                {
                    objects[b].curDir *= -1.15f;             
                }
            }
            
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
                acceleration += forceDir * 20f * objects[i].mass / dst;
            }
        }

        return acceleration;
    }

}
