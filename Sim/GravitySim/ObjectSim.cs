using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;

public class ObjSimulation : Game
{

    Object[] objects;


    //settings
    int numObjs = 20;
    float gravity = 2f;
    float collsionPushFactor = -1.15f;
    bool haveSun = false;
    bool randomPos = true;
    int posXChoose1 = 939;
    int posYChoose1 = 489;
    int posXChoose2 = 941;
    int posYChoose2 = 491;
    
    public void StRunSimulation()
    {
        objects = new Object[numObjs];
        //give objects settings
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = new Object();
            if(i == 0 && haveSun)
            {
                objects[i].mass = 100;
                int X = 900;
                int Y = 500;
                objects[i].pos = new Vector2(X, Y);
            }
            else
            {
                objects[i].mass = new Random().Next(6, 12);
                float posX;
                float posY;
                posX = new Random().Next(posXChoose1, posXChoose2);
                posY = new Random().Next(posYChoose1, posYChoose2);
                if(randomPos)
                {
                    posX = new Random().Next(0, 1900);
                    posY = new Random().Next(0, 1000);
                }
                objects[i].pos = new Vector2(posX, posY);
            }
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
                objects[i].UpdateColl();
            }
            else
            {
                Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
                objects[i].UpdateDir(acceleration);
                objects[i].UpdatePos();
                objects[i].UpdateColl();
            }

            for (int b = 0; b < objects.Length; b++)
            {
                if(b != i && Vector2.Distance(objects[i].pos, objects[b].pos) < 10)
                {
                    objects[b].curDir *= collsionPushFactor;             
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
                acceleration += forceDir * gravity * objects[i].mass / dst;
            }
        }

        return acceleration;
    }

}
