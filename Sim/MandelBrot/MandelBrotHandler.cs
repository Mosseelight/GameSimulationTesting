using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class MandelBrotHandler
    {
        int visualScaleX = 1;
        int visualScaleY = 1;
        int visualX;
        int visualY;
        Color[] colors;

        int Iterations = 10000;
        float MinX = -2.5f;
        float MaxX = 1f;
        float MinY = -1f;
        float MaxY = 1f;
        float xStep;
        float yStep;


        public void InitMandel(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScaleX;
            visualY = graphics.PreferredBackBufferHeight / visualScaleY;
            colors = new Color[visualX * visualY];
        }

        public void RunMandel()
        {
            xStep = (MaxX - MinX) / visualX;
            yStep = (MaxY - MinY) / visualY;

            Parallel.For (0, visualX, x =>
            {
                for (int y = 0; y < visualY; y++)
                {
                    float cr = MinX + x * xStep;
                    float ci = MinY + y * yStep;
                    float zr = 0;
                    float zi = 0;

                    int i;
                    for (i = 0; i < Iterations; i++)
                    {
                        float temp = zr * zr - zi * zi + cr;
                        zi = 2 * zr * zi + ci;
                        zr = temp;

                        if (zr * zr + zi * zi > 10)
                            break;
                    }

                    if (i == Iterations)
                        colors[y + visualY * x] = Color.Black;
                    else
                        colors[y + visualY * x] = new Color(i % 256, i % 256 / 2, i % 256 / 4);
                }
            });
        }


        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Camera camera)
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