using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Grapher
    {
        int visualScaleX = 5;
        int visualScaleY = 5;
        int visualX;
        int visualY;
        Color[] colors;


        int xPos;
        int yPos;


        public void InitGraph(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScaleX;
            visualY = graphics.PreferredBackBufferHeight / visualScaleY;
            colors = new Color[visualX * visualY];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(256f, 256f, 256f);
            }
        }

        public void RunGraph()
        {
            
        }


        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            for (int x = 0; x < visualX; x++)
            {
                for (int y = 0; y < visualY; y++) 
                {
                    spriteBatch.Draw(pixel, new Vector2(x * visualScaleX,y * visualScaleY), colors[y + visualY * x]);
                }
            }
            spriteBatch.End();
        }
    }
}