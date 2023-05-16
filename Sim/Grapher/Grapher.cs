using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Grapher
    {
        int visualScale = 3;
        int visualX;
        int visualY;
        Color[] colors;


        int xOffset;
        int yOffset;
        int graphIndex = 0;
        int drawIndex = 0;


        public void InitGraph(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScale;
            xOffset = visualX / 2;
            visualY = graphics.PreferredBackBufferHeight / visualScale;
            yOffset = visualY / 2;
            colors = new Color[visualX * visualY];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(256f, 256f, 256f);
            }
        }

        public void RunGraph()
        {
            for (int x = xOffset; x > -xOffset; x--)
            {
                for (int y = yOffset; y > -yOffset; y--)
                {
                    if(y == GraphingFunction(-x))
                    {
                        colors[graphIndex] = Color.Red;
                    }
                    graphIndex++;
                }
            }
        }

        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Camera camera)
        {
            spriteBatch.Begin();
            for (int x = 0; x < visualX; x++)
            {
                for (int y = 0; y < visualY; y++) 
                {
                    spriteBatch.Draw(pixel, new Vector2(x * visualScale, y * visualScale), colors[y + visualY * x]);
                }
            }
            spriteBatch.End();
        }


        int GraphingFunction(int x)
        {
            double value = x*x*x;
            return (int)Math.Round(value);
        }
    }
}