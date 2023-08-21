using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PixelSimElement;
using System.Collections.Generic;

namespace GameTesting
{
    public class PixelSim
    {
        PixelDrawer pixelDrawer = new PixelDrawer();
        List<Element> elements = new List<Element>();
        int[] positionCheck;

        public void InitPixelSim(GraphicsDeviceManager graphics)
        {
            pixelDrawer.visualScale = 4;
            pixelDrawer.InitDrawer(graphics);
            positionCheck = new int[pixelDrawer.xTotal * pixelDrawer.yTotal];
            for (int i = 0; i < pixelDrawer.colors.Length; i++)
            {
                pixelDrawer.colors[i] = Color.DarkGray;
            }
            for (int i = 0; i < 100; i++)
            {
                elements.Add(new WallPE());
                elements[i].id = i;
                elements[i].position = new Vector2((int)(660 / pixelDrawer.visualScale + i), (int)(600 / pixelDrawer.visualScale + i / 3));
                positionCheck[pixelDrawer.GetIndexOnPos(elements[i].position)] = 1;
            }
            for (int i = 100; i < 1000; i++)
            {
                elements.Add(new SandPE());
                elements[i].id = i;
                elements[i].position = new Vector2((int)(850 / pixelDrawer.visualScale), (int)(200 / pixelDrawer.visualScale));
                positionCheck[pixelDrawer.GetIndexOnPos(elements[i].position)] = 1;
            }
        }

        public void RunPixelSim()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Update(ref elements, ref positionCheck, ref pixelDrawer);
                if(!elements[i].BoundsCheck(0, 0, pixelDrawer.xTotal, pixelDrawer.yTotal))
                    pixelDrawer.colors[pixelDrawer.GetIndexOnPos(elements[i].position)] = elements[i].color;
            }
        }

        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            pixelDrawer.DrawPixels(ref pixel, spriteBatch, graphics);
            for (int i = 0; i < pixelDrawer.colors.Length; i++)
            {
                pixelDrawer.colors[i] = Color.DarkGray;
            }
        }
    }
}