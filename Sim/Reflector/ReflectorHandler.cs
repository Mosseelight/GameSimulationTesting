using System;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class ReflectorHandler
    {

        PixelDrawer pixelDrawer = new PixelDrawer();
        int[] wallCells;
        int relfections = 2;

        public void InitReflector(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 5;
            pixelDrawer.InitDrawer(graphics);

            wallCells = new int[pixelDrawer.yTotal];
            for (int i = 0; i < wallCells.Length; i++)
            {
                wallCells[i] = (pixelDrawer.yTotal * pixelDrawer.xOffset - 1) - i;
                pixelDrawer.colors[(pixelDrawer.yTotal * pixelDrawer.xOffset - 1) - i] = Color.Gray;
            }
        }

        public void DrawReflectorLine()
        {
            Vector2 lineOrigin = Vector2.Zero;
            Vector2 lineDir = new Vector2(0.5f,0.5f);
            Vector2 linePos;
            linePos = lineOrigin;
            for (int r = 0; r < relfections; r++)
            {
                while(true)
                {
                    int index = (pixelDrawer.yTotal * (int)linePos.X) + (int)linePos.Y;
                    pixelDrawer.colors[index] = Color.Green;
                    linePos += lineDir;
                    if(!wallCells.Contains(index + (pixelDrawer.yTotal * r)))
                    {
                        pixelDrawer.colors[index] = Color.Green;
                        linePos += lineDir;
                    }
                    else 
                    {
                        lineDir = new Vector2(-1f, 0f);
                        break;
                    }
                }
            }
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}