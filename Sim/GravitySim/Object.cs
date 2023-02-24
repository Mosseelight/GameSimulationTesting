using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Object : Game
{

    public float mass {get; set;}
    public Vector2 pos {get; set;}
    public Vector2 curDir {get; set;}
    public Vector2 startDir {get; set;}

    private GameTime time = new GameTime();

    public void start()
    {
        curDir = startDir;
    }

    public void UpdateDir(Vector2 acceleration)
    {
        curDir += acceleration;
    }

    public void UpdatePos()
    {
        pos += curDir;
    }

    public void UpdateColl(Vector2 acceleration)
    {
        if(pos.X > 780 || pos.X < 0 || pos.Y > 460 || pos.Y < 0)
        {
            curDir *= -1;
        }
    }

}