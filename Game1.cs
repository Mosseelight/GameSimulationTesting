using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameTesting
{
    public class Game1 : Game
    {

        float time;

        float MarioX;
        float MarioY;
        Texture2D Mario;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Mario = Content.Load<Texture2D>("mario-removebg-preview");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.W)) { MarioY--; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { MarioY++; }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { MarioX--; }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { MarioX++; }



            base.Update(gameTime);
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            gravity();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(Mario, new Vector2(MarioX, MarioY), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        //input a paramater to affect
        /*equation is V(Max falling speed) = Sqaure root of 2mg/pACd
         * 
         * m is thr mass of the object
         * g is gravity const
         * p is density of fluid its fallin through so air is 1.204
         * A is the area the object covers
         * Cd is the drag coefficent of the object so lets say 1
         * 
         * 
         * 
         * Y is V
         * 
         * 
         */
        public void gravity()
        {

            const double g = 9.18;
            const float p = 1.204f;
            const double Cd = 1;
            float Mass = 80f;
            float Area = 1f;
            double Y;

            //max velicty is 9.81 m/s
            Y = Math.Sqrt(2 * Mass * g / p * Area * Cd);
            MarioY -= Convert.ToSingle(Y) * -1 * time;
            Debug.WriteLine(Y);
            
        }
    }
}