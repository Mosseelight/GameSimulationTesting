using System;
using System.Linq;
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
        int parentIndex = 0;
        bool foundEnd = false;

        public void InitPF(GraphicsDeviceManager graphics)
        {
            pixelDrawer = new PixelDrawer();
            pixelDrawer.visualScale = 10;
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
            //start of check
            int c = 1;
            while (foundEnd == false)
            {
                int indexToCheck = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        //left
                        indexToCheck = ((parentIndex) - pixelDrawer.yOffset * 2);
                    }
                    if (i == 1)
                    {
                        //right
                        indexToCheck = ((parentIndex) + pixelDrawer.yOffset * 2);
                    }
                    if (i == 2)
                    {
                        //down
                        indexToCheck = (parentIndex + 1);
                    }
                    if (i == 3)
                    {
                        //up
                        indexToCheck = (parentIndex - 1);
                    }
                    if (indexToCheck == endPIndex && indexToCheck < pixelDrawer.colors.Length && indexToCheck > -1)
                    {
                        pixelDrawer.colors[indexToCheck] = new Color(0f, 0f, 0f);
                        foundEnd = true;
                    }
                    else if (!checkedCells.Contains(indexToCheck) && indexToCheck < pixelDrawer.colors.Length && indexToCheck > -1)
                    {
                        pixelDrawer.colors[indexToCheck] = new Color(0.1f, 0.5f, 0.1f);
                        checkedCells.Add(indexToCheck);
                    }
                }
                parentIndex = checkedCells[c + 1];
                c++;
            }
        }



        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}