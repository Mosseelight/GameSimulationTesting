﻿using Microsoft.Xna.Framework;
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
        private SpriteFont spriteFont;
        Vector2 fontPos;

        //simulation vars
        ObjSimulation objSim = new ObjSimulation();

        Texture2D circle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //_graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            objSim.StRunSimulation();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            circle = Content.Load<Texture2D>("circle");
            Content.Load<SpriteFont>("Arial");
            fontPos = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here
            objSim.UpRunSimulation();

            if(Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Saver saver = new Saver();
                saver.SaveSimSettings();
            }


            base.Update(gameTime);
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            // TODO: Add your drawing code here

            objSim.DrawSim(circle, spriteBatch, _graphics, spriteFont);


            base.Draw(gameTime);
        }
    }
}