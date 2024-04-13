using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace GameTesting
{
    public class PathFinderHandler
    {
        PixelDrawer pixelDrawer;

        int startPIndex;
        int endPIndex;

        List<int> cells;
        int parentIndex = 3;
        bool foundEnd = false;

        public void InitPF(GraphicsDeviceManager graphics)
        {
            pixelDrawer = new PixelDrawer();
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);
            cells = new List<int>();
            for (int i = 0; i < pixelDrawer.colors.Length; i++)
            {
                cells.Add(0);
            }
            startPIndex = 0;
            endPIndex = pixelDrawer.colors.Length - 1;
            cells[startPIndex] = 10;
            cells[endPIndex] = 11;
            //start 
            pixelDrawer.colors[startPIndex] = new Color(0f, 256f, 0f);
            //end
            pixelDrawer.colors[endPIndex] = new Color(256f, 0f, 0f);
        }
        //the pixels positions goes top down then moves one right
        //so the center is (pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset) - 1 - pixelDrawer.yOffset
        //to move over one right or left is to add or subtract from the xoffset
        //to move up and down you plus or minus on the second yoffset
        //the first yoffset is to get the middle

        // -1 on x goes left
        // +1 on x goes right
        // -1 on y goes down
        // +1 on y goes up

        public async void RunPathFinder()
        {
            parentIndex = startPIndex;
            Vector2 pos = pixelDrawer.GetPosOnIndex(parentIndex);
            for (int c = 0; !foundEnd; c++)
            {
                int indexToCheck = 0;
                Vector2 posCheck = Vector2.Zero;
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            posCheck = pos - new Vector2(1,0);
                            break;
                        case 1:
                            posCheck = pos - new Vector2(0,1);
                            break;
                        case 2:
                            posCheck = pos + new Vector2(1,0);
                            break;
                        case 3:
                            posCheck = pos - new Vector2(0,1);
                            break;
                    }
                    indexToCheck = pixelDrawer.GetIndexOnPos(posCheck);
                    if(indexToCheck < pixelDrawer.colors.Length && indexToCheck > -1)
                    {
                        if (indexToCheck == endPIndex)
                        {
                            pixelDrawer.colors[indexToCheck] = new Color(256f, 0f, 0f);
                            foundEnd = true;
                        }
                        else if (cells[indexToCheck] == 0)
                        {
                            pixelDrawer.colors[indexToCheck] = new Color(cells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale), cells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale), cells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale));
                            cells[indexToCheck] = 1;
                        }
                    }
                }
                Console.WriteLine(posCheck);
                await Task.Delay(1);
                pos = posCheck;
            }
        }

        public void Draw(ref Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(ref pixel, spriteBatch, graphics);
        }
    }
}