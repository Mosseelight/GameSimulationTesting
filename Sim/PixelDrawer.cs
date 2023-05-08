using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class PixelDrawer
    {
        public int visualScale;
        int visualX;
        int visualY;
        public Color[] colors;


        public int xOffset;
        public int xTotal;
        public int yOffset;
        public int yTotal;
        int drawIndex = 0;


        public void InitDrawer(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScale;
            xTotal = graphics.PreferredBackBufferWidth / visualScale;
            xOffset = visualX / 2;
            visualY = graphics.PreferredBackBufferHeight / visualScale;
            yTotal = graphics.PreferredBackBufferHeight / visualScale;
            yOffset = visualY / 2;
            colors = new Color[visualX * visualY];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(256f, 256f, 256f);
            }
        }

        public void LineDrawer(Vector2 lineDir, int lineLength, Vector2 lineOrigin, Color lineColor, int[] stopList)
        {
            Vector2 linePos;
            linePos = lineOrigin;
            for (int i = 0; i < lineLength; i++)
            {
                int index = (yTotal * (int)linePos.X) + (int)linePos.Y;
                colors[index] = lineColor;
                linePos += lineDir;
                if(!stopList.Contains(index + yTotal))
                {
                    colors[index] = lineColor;
                    linePos += lineDir;
                }
                else 
                {
                    break;
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
                    spriteBatch.Draw(pixel, new Vector2(x * visualScale, y * visualScale), null, colors[y + visualY * x], 0, Vector2.Zero, new Vector2(visualScale, visualScale), SpriteEffects.None, 0f);
                    drawIndex++;
                }
            }
            spriteBatch.End();
        }

        public Vector2 GetPosOnIndex(int index)
        {
            return new Vector2((float)Math.Floor((double)((index % yTotal) * visualScale)), (float)Math.Floor((double)((index / yTotal) * visualScale)));
        }
    }
}