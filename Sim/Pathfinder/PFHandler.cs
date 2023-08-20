using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class PathFinderHandler
    {
        PixelDrawer pixelDrawer;

        int startPIndex;
        int endPIndex;

        List<int> checkedCells = new List<int>();
        int parentIndex = 3;
        bool foundEnd = false;

        public void InitPF(GraphicsDeviceManager graphics)
        {
            pixelDrawer = new PixelDrawer();
            pixelDrawer.visualScale = 3;
            pixelDrawer.InitDrawer(graphics);
            startPIndex = new Random().Next(0, pixelDrawer.colors.Length - 1);
            endPIndex = new Random().Next(0, pixelDrawer.colors.Length - 1);
            checkedCells.Add(startPIndex);
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

        public void RunPathFinder()
        {
            parentIndex = startPIndex;
            for (int c = 1; !foundEnd; c++)
            {
                int indexToCheck = 0;
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            indexToCheck = (parentIndex - 1);
                            break;
                        case 1:
                            indexToCheck = ((parentIndex) - pixelDrawer.yTotal);
                            break;
                        case 2:
                            indexToCheck = ((parentIndex) + pixelDrawer.yTotal);
                            break;
                        case 3:
                            indexToCheck = (parentIndex + 1);
                            break;
                    }
                    if (indexToCheck == endPIndex && indexToCheck < pixelDrawer.colors.Length && indexToCheck > -1)
                    {
                        pixelDrawer.colors[indexToCheck] = new Color(256f, 0f, 0f);
                        foundEnd = true;
                    }
                    else if (!checkedCells.Contains(indexToCheck) && indexToCheck < pixelDrawer.colors.Length && indexToCheck > -1)
                    {
                        pixelDrawer.colors[indexToCheck] = new Color(checkedCells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale), checkedCells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale), checkedCells.Count / (pixelDrawer.xOffset * pixelDrawer.visualScale));
                        checkedCells.Add(indexToCheck);
                    }
                }

                //await Task.Delay(1);
                parentIndex = checkedCells[c+1];
            }
        }

        public void Draw(ref Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(ref pixel, spriteBatch, graphics);
        }
    }
}