using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        VectorFieldSim vectorField = new VectorFieldSim();
        NeuralNetworkHandler neuralNetwork = new NeuralNetworkHandler();

        Texture2D circle;
        Texture2D arrow;
        Texture2D pixel;


        //game switching
        bool runGravitySim = false;
        bool runVectorFieldSim = false;
        bool runNerualNetworkSim = false;
        bool Initializer = false;


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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            circle = this.Content.Load<Texture2D>("circle");
            arrow = this.Content.Load<Texture2D>("arrow");
            font = this.Content.Load<SpriteFont>("Arial");
            pixel = this.Content.Load<Texture2D>("pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            //select which sim to run
            if(!Initializer)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.D1) && !runGravitySim)
                {
                    runGravitySim = true;
                    Initializer = true;
                    objSim.StRunSimulation();
                    objSim.SimApplySettings();
                    Console.WriteLine("pressed 1");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D2) && !runVectorFieldSim)
                {
                    runVectorFieldSim = true;
                    Initializer = true;
                    vectorField.CreateVectorField();
                    Console.WriteLine("pressed 2");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D3) && !runNerualNetworkSim)
                {
                    runNerualNetworkSim = true;
                    Initializer = true;
                    neuralNetwork.InitNeuralNetwork(_graphics);
                    neuralNetwork.RunNerualNetwork();
                    Console.WriteLine("pressed 3");
                }
            }

            // TODO: Add your update logic here
            if(runGravitySim)
            {
                objSim.HandleInput();
                objSim.UpRunSimulation();
            }
            if(runVectorFieldSim)
            {
                vectorField.HandleInput();
                vectorField.ApplyFieldDirection();
            }
            if(runNerualNetworkSim)
            {
                neuralNetwork.HandleInput();
            }

            base.Update(gameTime);
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);
            // TODO: Add your drawing code here
            if(runGravitySim)
            {
                objSim.DrawSim(circle, spriteBatch, _graphics, font);
            }
            if(runVectorFieldSim)
            {
                vectorField.DrawSim(circle, arrow, spriteBatch, _graphics, font);
            }
            if(runNerualNetworkSim)
            {
                neuralNetwork.DrawPixels(pixel, spriteBatch, _graphics);
            }


            base.Draw(gameTime);
        }
    }
}