using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class PathFinderHandler
    {
        PixelDrawer pixelDrawer;

        public void initPF(GraphicsDeviceManager graphics)
        {
           pixelDrawer = new PixelDrawer();
           pixelDrawer.visualScale = 5;
           pixelDrawer.InitDrawer(graphics);
           pixelDrawer.colors[0] = new Color(0f,0f,0f);
           pixelDrawer.colors[(pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset) - 1 - (pixelDrawer.yOffset)] = new Color(0f,0f,0f);
           pixelDrawer.colors[(pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset - 1) - 1 - (pixelDrawer.yOffset)] = new Color(0f,256f,0f);
           pixelDrawer.colors[(pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset + 1) - 1 - (pixelDrawer.yOffset)] = new Color(256f,0f,0f);
           pixelDrawer.colors[(pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset) - 1 - (pixelDrawer.yOffset - 1)] = new Color(0f,0f,256f);
           pixelDrawer.colors[(pixelDrawer.yOffset * 2) * (pixelDrawer.xOffset) - 1 - (pixelDrawer.yOffset + 1)] = new Color(256f,0f,256f);
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

        

        public void Draw(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(pixel, spriteBatch, graphics);
        }
    }
}