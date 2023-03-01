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
        private SpriteFont font;

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
            objSim.SimApplySettings();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            circle = this.Content.Load<Texture2D>("circle");
            font = this.Content.Load<SpriteFont>("Arial");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Console.WriteLine("test");
                Saver saver = new Saver();
                saver.SaveSimSettings();
                Console.WriteLine("Saved!");
            }
            objSim.HandleInput();

            base.Update(gameTime);
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            objSim.UpRunSimulation();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            // TODO: Add your drawing code here

            objSim.DrawSim(circle, spriteBatch, _graphics, font);


            base.Draw(gameTime);
        }
    }
}