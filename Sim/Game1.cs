﻿using Microsoft.Xna.Framework;
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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        Camera camera = new Camera(new Vector3(0, 0, 0), Vector3.Zero, 1f);

        //simulation vars
        ObjSimulation objSim = new ObjSimulation();
        VectorFieldSim vectorField = new VectorFieldSim();
        NeuralNetworkHandlerVisual neuralNetwork = new NeuralNetworkHandlerVisual();
        PixelSim pixelSim = new PixelSim();
        Grapher grapher = new Grapher();
        PathFinderHandler pathFinder = new PathFinderHandler();
        RendererHandler renderer = new RendererHandler();
        RayTracerHandler rayTracer = new RayTracerHandler();
        ReflectorHandler reflector = new ReflectorHandler();
        EvoSim evoSim = new EvoSim();

        Texture2D circle;
        Texture2D arrow;
        Texture2D pixel;


        //game switching
        bool runGravitySim = false;
        bool runVectorFieldSim = false;
        bool runNerualNetworkSim = false;
        bool runPixelSim = false;
        bool runGraphSim = false;
        bool runPathSim = false;
        bool runRendererSim = false;
        bool runRayTracerSim = false;
        bool runReflectorSim = false;
        bool runEvoSim = false;
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
            arrow = this.Content.Load<Texture2D>("Arrow");
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
                if(Keyboard.GetState().IsKeyDown(Keys.D4) && !runEvoSim)
                {
                    Initializer = true;
                    runEvoSim = true;
                    evoSim.InitEvoSim();
                    Console.WriteLine("pressed 4");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D5) && !runPixelSim)
                {
                    runPixelSim = true;
                    Initializer = true;
                    pixelSim.InitPixelSim(_graphics);
                    Console.WriteLine("pressed 5");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D6) && !runGraphSim)
                {
                    runGraphSim = true;
                    Initializer = true;
                    grapher.InitGraph(_graphics);
                    grapher.RunGraph();
                    Console.WriteLine("pressed 6");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D7) && !runPathSim)
                {
                    runPathSim = true;
                    Initializer = true;
                    pathFinder.InitPF(_graphics);
                    pathFinder.RunPathFinder();
                    Console.WriteLine("pressed 7");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D8) && !runRendererSim)
                {
                    runRendererSim = true;
                    Initializer = true;
                    renderer.InitRenderer(_graphics);
                    Console.WriteLine("pressed 8");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D9) && !runReflectorSim)
                {
                    runReflectorSim = true;
                    Initializer = true;
                    reflector.InitReflector(_graphics);
                    reflector.DrawReflectorLine();
                    Console.WriteLine("pressed 9");
                }
                if(Keyboard.GetState().IsKeyDown(Keys.D0) && !runRayTracerSim)
                {
                    runRayTracerSim = true;
                    Initializer = true;
                    rayTracer.InitRayTracer(_graphics);
                    Console.WriteLine("pressed 0");
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
            if(runEvoSim)
            {
                evoSim.UpdateEvoSim();
            }
            if(runPixelSim)
            {
                pixelSim.RunPixelSim();
            }

            base.Update(gameTime);
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            //Stopwatch stopwatch = Stopwatch.StartNew();
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
            if(runPixelSim)
            {
                pixelSim.DrawPixels(pixel, spriteBatch, _graphics);
            }
            if(runGraphSim)
            {
                grapher.DrawPixels(pixel, spriteBatch, _graphics, camera);
            }
            if(runPathSim)
            {
                pathFinder.Draw(ref pixel, spriteBatch, _graphics);
            }
            if(runRendererSim)
            {
                renderer.Draw(ref pixel, spriteBatch, _graphics, time);
            }
            if(runRayTracerSim)
            {
                rayTracer.Draw(ref pixel, spriteBatch, _graphics);
            }
            if(runReflectorSim)
            {
                reflector.Draw(ref pixel, spriteBatch, _graphics);
            }
            if(runEvoSim)
            {
                evoSim.DrawEvoSim(circle, spriteBatch, _graphics);
            }
            //stopwatch.Stop();
            //long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            //Console.WriteLine("Rendering took " + elapsedMilliseconds + " ms or " + 1000.0f / elapsedMilliseconds + "fps");
            base.Draw(gameTime);
        }
    }
}