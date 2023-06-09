using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class EvoSim
    {

        List<Animal> animals = new List<Animal>();
        int animalAmount = 1;

        public void InitEvoSim()
        {
            for (int i = 0; i < animalAmount; i++)
            {
                animals.Add(new Animal(5, 5, 0, 100, 100, new Vector2(1000, 500), Color.White));
            }
        }

        public void UpdateEvoSim()
        {
            //run on a update tick? like 500ms then update
            for (int i = 0; i < animalAmount; i++)
            {
                animals[i].UpdateAnimal();
            }
        }

        public void DrawEvoSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            for (int i = 0; i < animalAmount; i++)
            {
                if(!animals[i].alive)
                    spriteBatch.Draw(circle, animals[i].position, animals[i].color);
            }
            spriteBatch.End();
        }
    }

    public class Animal
    {
        //stats
        public float speed = 1;
        public float visualRadius = 5;
        public float preyLevel = 0; //This repsresents the prey level that the animal will have. A lower value means an animal with a higher value can eat the lower value. Lower value animals will run away from higher value animals 
        public float hungerCutoff = 100;
        public float thirstCutoff = 100;
        public bool alive = true;
        public Color color = Color.White;

        //changing stats
        float hunger = 0; //low value means satisfied hunger. High value means hungry
        float thirst = 0; //Same as hunger
        Vector2 moveDir = Vector2.Zero;
        public Vector2 position = Vector2.Zero;

        //stuff
        static float RandomNumber(float min, float max)
        {
            Random random = new Random();
            float val = (random.NextSingle() * (max - min) + min);
            return val;
        }

        public Animal() {}

        public Animal(float speed, float visualRadius, float preyLevel, float hungerCutoff, float thirstCutoff, Vector2 position, Color color)
        {
            this.speed = speed;
            this.visualRadius = visualRadius;
            this.preyLevel = preyLevel;
            this.hungerCutoff = hungerCutoff;
            this.thirstCutoff = thirstCutoff;
            this.position = position;
            this.color = color;
        }

        public void UpdateAnimal()
        {
            UpdateStats();
            UpdateMovement();
        }

        void UpdateStats()
        {
            if(hunger > hungerCutoff || thirst > thirstCutoff)
            {
                alive = false;
            }
            hunger++;
            thirst++;
        }

        void UpdateMovement()
        {
            moveDir = new Vector2(RandomNumber(-1, 1), RandomNumber(-1, 1));
            position += moveDir * speed;
        }

    }
}