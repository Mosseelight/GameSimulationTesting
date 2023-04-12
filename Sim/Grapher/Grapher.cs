using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Grapher
    {
        int visualScale = 5;
        int visualX;
        int visualY;
        Color[] colors;


        int xPos;
        int xOffset;
        int yPos;
        int yOffset;


        public void InitGraph(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScale;
            xOffset = (graphics.PreferredBackBufferWidth / 2) / visualScale;
            visualY = graphics.PreferredBackBufferHeight / visualScale;
            yOffset = (graphics.PreferredBackBufferHeight / 2) / visualScale;
            colors = new Color[visualX * visualY];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(256f, 256f, 256f);
            }
            Console.WriteLine(xOffset + " " + yOffset);
        }

        public void RunGraph()
        {
            for (int x = 0; x < visualX; x++)
            {
                for (int y = 0; y < visualY; y++) 
                {
                    if(y == GraphingFunction(x)) 
                    {
                        colors[y + visualY * x] = new Color(0f,0f,0f);
                    }
                }
            }
        }

        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            for (int x = 0; x < visualX; x++)
            {
                for (int y = 0; y < visualY; y++) 
                {
                    spriteBatch.Draw(pixel, new Vector2(x * visualScale,y * visualScale), colors[y + visualY * x]);
                }
            }
            spriteBatch.End();
        }


        int GraphingFunction(int inputX)
        {
            float value = (float)Math.Sin(inputX);
            return (int)value;
        }
    }
}