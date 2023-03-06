using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Object
{

    public float mass {get; set;}
    public Vector2 pos {get; set;}
    public Vector2 curDir {get; set;}
    public Vector2 startDir {get; set;}
    public bool enabled {get; set;}
    public bool isSun = false;

    public void start()
    {
        curDir = startDir;
    }

    public void UpdateDir(Vector2 acceleration)
    {
        curDir = acceleration;
    }

    public void UpdatePos()
    {
        pos += curDir;
    }

    public void UpdateColl()
    {
        if(pos.X > 1900 || pos.X < 0 || pos.Y > 1000 || pos.Y < 0)
        {
            if(pos.X > 1900)
            {
                pos = new Vector2(1900,pos.Y);
            }
            if(pos.X < 00)
            {
                pos = new Vector2(0,pos.Y);
            }
            if(pos.Y > 1000)
            {
                pos = new Vector2(pos.X,1000);
            }
            if(pos.Y < 0)
            {
                pos = new Vector2(pos.X,0);
            }
            curDir = new Vector2(curDir.X * -0.7f, curDir.Y * -0.7f);
        }
    }

}