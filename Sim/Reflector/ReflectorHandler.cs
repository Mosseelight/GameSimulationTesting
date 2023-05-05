using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class ReflectorHandler
    {

        PixelDrawer pixelDrawer = new PixelDrawer();
        int[] wallCells;
        int relfections = 10;

        public void InitReflector(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 5;
            pixelDrawer.InitDrawer(graphics);

            wallCells = new int[pixelDrawer.yTotal * pixelDrawer.xTotal];
            
            for (int i = 0; i < pixelDrawer.yTotal; i++)
            {
                wallCells[i] = i;
                pixelDrawer.colors[i] = Color.Gray;
                wallCells[i] = (pixelDrawer.yTotal * pixelDrawer.xTotal - 1) - i;
                pixelDrawer.colors[(pixelDrawer.yTotal * pixelDrawer.xTotal - 1) - i] = Color.Gray;
            }
            for (int i = 0; i < pixelDrawer.xTotal; i++)
            {
                wallCells[i] = (pixelDrawer.yTotal * pixelDrawer.xTotal - 1) - pixelDrawer.yTotal * i;
                pixelDrawer.colors[(pixelDrawer.yTotal * pixelDrawer.xTotal - 1) - pixelDrawer.yTotal * i] = Color.Gray;
            }

            
        }

        public async void DrawReflectorLine()
        {
            Vector2 lineOrigin = new Vector2(5, 5);
            Vector2 lineDir = new Vector2(1f,1f);
            Vector2 linePos;
            bool hitReflectorCell;
            for (int r = 0; r < relfections; r++)
            {
                hitReflectorCell = false;
                linePos = lineOrigin;
                while(!hitReflectorCell)
                {
                    int index = (pixelDrawer.yTotal * (int)linePos.X) + (int)linePos.Y;
                    if(!wallCells.Contains(index) && !CheckBounds(linePos) && index < pixelDrawer.colors.Length && index > -1)
                    {
                        pixelDrawer.colors[index] = Color.Green;
                        linePos += lineDir;
                    }
                    else 
                    {
                        lineOrigin = linePos - new Vector2((float)Math.Ceiling(lineDir.X), (float)Math.Ceiling(lineDir.Y));
                        lineDir = ReturnSrufaceDirection(lineDir, GetPosOnIndex(index), index);
                        hitReflectorCell = true;
                    }
                    await Task.Delay(1);
                }
            }
        }

        bool CheckBounds(Vector2 pos)
        {
            if(pos.X > pixelDrawer.xTotal || pos.X < 0 || pos.Y > pixelDrawer.yTotal || pos.Y < 0)
            {
                return true;
            }
            return false;
        }

        Vector2 ReturnSrufaceDirection(Vector2 dir, Vector2 pos, int pIndex)
        {           
            int leftIndex = pIndex - pixelDrawer.yTotal;
            int rightIndex = pIndex + pixelDrawer.yTotal;
            int upIndex = pIndex - 1;
            int downIndex = pIndex + 1;
            Random random = new Random();
            

            //horizontal
            if(wallCells.Contains(leftIndex) && wallCells.Contains(rightIndex))
            {
                return new Vector2(dir.X, -dir.Y);
            }
            //vertical
            else 
            {
                return new Vector2(-dir.X, dir.Y);
            }
        }

        Vector2 GetPosOnIndex(int index)
        {
            return new Vector2(((index / pixelDrawer.xTotal) * pixelDrawer.yTotal * pixelDrawer.visualScale), ((index % pixelDrawer.xTotal) * pixelDrawer.visualScale));
        }

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}