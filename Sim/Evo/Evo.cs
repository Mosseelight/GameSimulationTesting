using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class EvoSim
    {
        List<Entity> entities = new List<Entity>();
        int animalAmount = 10;
        int plantAmount = 10;

        public void InitEvoSim()
        {
            for (int i = 0; i < animalAmount; i++)
            {
                entities.Add(new Entity(new Animal(5, 100, 0, 100, 100, new Vector2(1000, 500), Color.White)));
            }

            for (int i = 0; i < plantAmount; i++)
            {
                entities.Add(new Entity(new Plant(new Vector2(900 - i, 400 + i))));
            }
        }

        public void UpdateEvoSim()
        {
            //run on a update tick? like 500ms then update
            for (int i = 0; i < animalAmount; i++)
            {
                if(entities[i].isAnimal)
                    entities[i].animal.UpdateAnimal(entities);
            }
            
        }

        public void DrawEvoSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            for (int i = 0; i < animalAmount; i++)
            {
                if(!entities[i].animal.alive)
                    spriteBatch.Draw(circle, entities[i].animal.position, entities[i].animal.color);
            }

            for (int i = 0; i < plantAmount; i++)
            {
                if(entities[i].isPlant)
                    spriteBatch.Draw(circle, entities[i].plant.position, Color.Green);
            }
            spriteBatch.End();
        }
    }

    public class Entity
    {
        public Animal animal;
        public bool isAnimal = false;
        public Plant plant;
        public bool isPlant = false;

        public Entity(Animal animal)
        {
            this.animal = animal;
            this.isAnimal = true;
            this.plant = null;
        }

        public Entity(Plant plant)
        {
            this.plant = plant;
            this.isPlant = true;
            this.animal = null;
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

        public void UpdateAnimal(List<Entity> entities)
        {
            UpdateStats();
            UpdateMovement();
            UpdateVisual(entities);
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
            position += moveDir * speed;
        }

        void UpdateVisual(List<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if(entities[i].isPlant && Vector2.Distance(position, entities[i].plant.position) < visualRadius)
                {
                    moveDir = Vector2.Normalize(entities[i].plant.position - position);
                }
                else
                {
                    moveDir = new Vector2(RandomNumber(-1, 1), RandomNumber(-1, 1));
                }

                if(entities[i].isAnimal && entities[i].animal.preyLevel > preyLevel && Vector2.Distance(position, entities[i].animal.position) < visualRadius)
                {
                    moveDir = Vector2.Normalize(position - entities[i].animal.position);
                }
            }
        }

    }

    public class Plant
    {
        public float nutrition;
        public Vector2 position;

        public Plant() {}

        public Plant(Vector2 position)
        {
            this.position = position;
        }
    }
}