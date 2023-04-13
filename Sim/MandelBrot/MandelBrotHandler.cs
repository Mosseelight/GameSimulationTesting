using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting
{
    public class MandelBrotHandler
    {
        int visualScaleX = 5;
        int visualScaleY = 5;
        int visualX;
        int visualY;
        Color[] colors;


        public void InitMandel(GraphicsDeviceManager graphics)
        {
            visualX = graphics.PreferredBackBufferWidth / visualScaleX;
            visualY = graphics.PreferredBackBufferHeight / visualScaleY;
            colors = new Color[visualX * visualY];
        }

        public void RunMandel()
        {
            
        }


        public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Camera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(graphics));
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