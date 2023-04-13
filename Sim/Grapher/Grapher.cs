using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class Grapher
    {
        int visualScale = 10;
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
            for (int x = 0; x < visualX; x++)
            {
                for (int y = 0; y < visualY; y++) 
                {
                    if(Math.Abs(y - GraphingFunction(x)) < 0.1) 
                    {
                        colors[y + visualY * x] = new Color(0f,0f,0f);
                    }
                }
            }

            /*for (int y = -yOffset; y < yOffset; y++)
            {
                for (int x = -xOffset; x < xOffset; x++) 
                {
                    if(Math.Abs(y - GraphingFunction(x)) < 0.1) 
                    {
                        Console.WriteLine(GraphingFunction(x));
                        colors[graphIndex + (((visualX * visualY) / 2) - (yOffset))] = new Color(0f,0f,0f);
                        graphIndex++;
                    }
                }
            }*/
        }

        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Camera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(graphics));
            for (int x = -xOffset; x < xOffset; x++)
            {
                for (int y = -yOffset; y < yOffset; y++) 
                {
                    spriteBatch.Draw(pixel, new Vector2(x * visualScale, y * visualScale), colors[y + visualY * x]);
                    drawIndex++;
                }
            }
            spriteBatch.End();
        }


        int GraphingFunction(int inputX)
        {
            float value = inputX * inputX * 0.1f;
            return (int)value;
        }
    }
}