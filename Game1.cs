using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;

namespace GameTesting
{

    public class Game1 : Game
    {

        float time;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        public static float MarioX;
        public static float MarioY;
        float speed = 10;

        public Texture2D Mario;

        public static bool gravity_Allow = true;

        Collision collision;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            collision = new Collision();

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

            if (Keyboard.GetState().IsKeyDown(Keys.W)) { MarioY-= 4; }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) { MarioY+= 4; }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) { MarioX-= 4; }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) { MarioX+= 4; }



            base.Update(gameTime);
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            collision.apply_Collision();
            gravity(80f);
            

            Debug.WriteLine(MarioY);
            if(MarioY >= 90)
            {
                Debug.WriteLine("below ground");
            }
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
        public void gravity(float Mass)
        {
            if(gravity_Allow = true)
            {
                const double g = 9.18;
            const float p = 1.204f;
            const double Cd = 1;
            float Area = 1f;
            double Y;

            //max velicty is 9.81 m/s
            Y = Math.Sqrt(2 * Mass * g / p * Area * Cd);
            MarioY -= Convert.ToSingle(Y) * -1 * time;
            }
            
        }
    }

    public class Collision
    {
        const float unit = 1f;
        const float halfUnit = 0.5f;
        const float ground_Y = 90f;

        public bool is_TouchingGround()
        {
            
            if(Game1.MarioY >= ground_Y)
            {
                return true;
            }

            return false;
        }

        public void apply_Collision()
        {
            if(is_TouchingGround())
            {
                Game1.MarioY = ground_Y;
                Game1.gravity_Allow = true;
            }
            else
            {
                Game1.gravity_Allow = false;
            }
        }


    }
}