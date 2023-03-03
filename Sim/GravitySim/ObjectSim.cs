using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameTesting
{

    public class ObjSimulation
    {
        Object[] objects;
        List<Object> objectsList = new List<Object>();
        Saver saver = new Saver();
        bool checkforoverlap = false;
        Random random = new Random();
        bool spawnKeyPressed = false;

        //settings
        int numObjs = 2;
        float gravity = 7f;
        float collsionPushFactor = -1.15f;
        bool haveSun = false;
        bool randomPos = true;
        int posXChoose = 950;
        int posYChoose = 500;
        int posTolerence = 50;
        float minCollideDist = 10f;
        float overlapCorrectionDist = 1f;
        int overlapAmountCheck = 1;
        float massCollideLoss = 5f;
        //increse the distance between the objects so that it would be technically farther away then it actually is for
        //a more realistic simulation rather than the object being 10 units away and zipping away
        float distScaleFactor = 50;

        public class ObjectSimSettings
        {
            public int numObjs { get; set; }
            public float gravity { get; set; }
            public float collsionPushFactor { get; set; }
            public bool haveSun { get; set; }
            public bool randomPos { get; set; }
            public int posXChoose { get; set; }
            public int posYChoose { get; set; }
            public int posTolerence { get; set; }
            public float minCollideDist { get; set; }
            public float overlapCorrectionDist { get; set; }
            public int overlapAmountCheck { get; set; }
            public float massCollideLoss { get; set; }
            public float distScaleFactor { get; set; }
        }

        public void StRunSimulation()
        {
            saver.CreateFolder();
            SaverDataToSet.objectSimSettingsSet.numObjs = numObjs;
            SaverDataToSet.objectSimSettingsSet.gravity = gravity;
            SaverDataToSet.objectSimSettingsSet.collsionPushFactor = collsionPushFactor;
            SaverDataToSet.objectSimSettingsSet.haveSun = haveSun;
            SaverDataToSet.objectSimSettingsSet.randomPos = randomPos;
            SaverDataToSet.objectSimSettingsSet.posXChoose = posXChoose;
            SaverDataToSet.objectSimSettingsSet.posYChoose = posYChoose;
            SaverDataToSet.objectSimSettingsSet.posTolerence = posTolerence;
            SaverDataToSet.objectSimSettingsSet.minCollideDist = minCollideDist;
            SaverDataToSet.objectSimSettingsSet.overlapCorrectionDist = overlapCorrectionDist;
            SaverDataToSet.objectSimSettingsSet.overlapAmountCheck = overlapAmountCheck;
            SaverDataToSet.objectSimSettingsSet.massCollideLoss = massCollideLoss;
            SaverDataToSet.objectSimSettingsSet.distScaleFactor = distScaleFactor;
            saver.SaveSimSettings();
            saver.ReadSimSettings();
        }

        public void SimApplySettings()
        {
            for (int i = 0; i < numObjs; i++)
            {
                objectsList.Add(new Object());
                objects = objectsList.ToArray();
                if (i == 0 && haveSun)
                {
                    objects[i].mass = 10000;
                    objects[i].isSun = true;
                    int X = 900;
                    int Y = 500;
                    objects[i].pos = new Vector2(X, Y);
                }
                else
                {
                    objects[i].mass = random.Next(6, 12);
                    float posX;
                    float posY;
                    posX = random.Next(posXChoose - posTolerence, posXChoose + posTolerence);
                    posY = random.Next(posYChoose - posTolerence, posYChoose + posTolerence);
                    if (randomPos)
                    {
                        posX = random.Next(0, 1900);
                        posY = random.Next(0, 1000);
                    }

                    objects[i].pos = new Vector2(posX, posY);
                }
                int randomXS = random.Next(10, 40);
                int randomYS = random.Next(10, 40);
                objects[i].startDir = new Vector2(randomXS, randomYS);
                objects[i].enabled = true;
                objects[i].start();
            }
            CheckOverlapSpawning();
        }

        public void UpRunSimulation()
        {
            Console.WriteLine(objectsList.Count + " List");
            Console.WriteLine(objects.Length + " Array");
            for(int i = 0; i < objects.Length; i++)
            {
                if (i == 0 && haveSun)
                {
                    Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
                    objects[i].UpdateDir(acceleration);
                    objects[i].UpdateColl();
                }
                else if (objects[i].enabled)
                {
                    Vector2 acceleration = CalculateAcceleration(objects[i].pos, objects[i]);
                    objects[i].UpdateDir(acceleration);
                    objects[i].UpdatePos();
                    objects[i].UpdateColl();
                }

                for (int b = 0; b < objects.Length; b++)
                {
                    //b != i checks if its self is the same as its self so it does not give a dist
                    //of 0
                    Console.WriteLine(i + " i");
                    Console.WriteLine(b + " b");
                    if (b != i && Vector2.Distance(objects[i].pos, objects[b].pos) < minCollideDist && objects[b].enabled && objects[i].enabled && !objects[i].isSun && !objects[b].isSun)
                    {
                        float tempMass = objects[b].mass + objects[i].mass - massCollideLoss;
                        Vector2 tempCurDir = objects[b].curDir - objects[i].curDir;
                        Vector2 tempPos = Vector2.Lerp(objects[b].pos, objects[i].pos, 0.5f);
                        objectsList.Remove(objects[b]);
                        objectsList.Remove(objects[i]);
                        objectsList.Add(new Object());
                        objects = objectsList.ToArray();
                        objects[objects.Length - 1].enabled = true;
                        objects[objects.Length - 1].mass = tempMass;
                        objects[objects.Length - 1].curDir = tempCurDir;
                        objects[objects.Length - 1].pos = tempPos;
                    }
                }

            }
        }

        //check when the objects are spawned that they are not spawning on top of each other
        private void CheckOverlapSpawning()
        {
            for(int x = 0; x < overlapAmountCheck; x++)
            {
                for(int i = 0; i < objects.Length; i++)
                {
                    if (!checkforoverlap)
                    {
                        for (int b = 0; b < objects.Length; b++)
                        {
                            if (i != b && objects[b].pos == objects[i].pos)
                            {
                                objects[i].pos += new Vector2(overlapCorrectionDist, overlapCorrectionDist);
                            }
                        }
                    }
                }
            }
            checkforoverlap = true;
        }

        public void DrawSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            Vector2 textMiddlePos = font.MeasureString("Gravity " + gravity) / 2;
            for (int i = 0; i < objects.Length; i++)
            {
                if(objects[i].enabled)
                {
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, "Gravity: " + gravity.ToString("N03"), new Vector2(textMiddlePos.X + 40, 1000), Color.Black, 0, textMiddlePos, 1.5f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(font, "CollPushFactor: " + collsionPushFactor.ToString("N03"), new Vector2(textMiddlePos.X + 40, 970), Color.Black, 0, textMiddlePos, 1.5f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(circle, objects[i].pos, Color.White);
                    spriteBatch.End();
                }
            }
        }

        public void HandleInput()
        {   
            if (Keyboard.GetState().IsKeyUp(Keys.G))
            {
                gravity += 0.005f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.H))
            {
                gravity -= 0.005f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.O))
            {
                collsionPushFactor += 0.005f;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.L))
            {
                collsionPushFactor -= 0.005f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Saver saver = new Saver();
                SaverDataToSet.objectSimSettingsSet.numObjs = numObjs;
                SaverDataToSet.objectSimSettingsSet.gravity = gravity;
                SaverDataToSet.objectSimSettingsSet.collsionPushFactor = collsionPushFactor;
                SaverDataToSet.objectSimSettingsSet.haveSun = haveSun;
                SaverDataToSet.objectSimSettingsSet.randomPos = randomPos;
                SaverDataToSet.objectSimSettingsSet.posXChoose = posXChoose;
                SaverDataToSet.objectSimSettingsSet.posYChoose = posYChoose;
                SaverDataToSet.objectSimSettingsSet.posTolerence = posTolerence;
                SaverDataToSet.objectSimSettingsSet.minCollideDist = minCollideDist;
                SaverDataToSet.objectSimSettingsSet.overlapCorrectionDist = overlapCorrectionDist;
                SaverDataToSet.objectSimSettingsSet.overlapAmountCheck = overlapAmountCheck;
                SaverDataToSet.objectSimSettingsSet.massCollideLoss = massCollideLoss;
                SaverDataToSet.objectSimSettingsSet.distScaleFactor = distScaleFactor;
                saver.SaveSimSettings();
                Console.WriteLine("Saved");
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Q) && !spawnKeyPressed)
            {
                spawnKeyPressed = true;
                objectsList.Add(new Object());
                objects = objectsList.ToArray();
                objects[objects.Length - 1].enabled = true;
                objects[objects.Length - 1].mass = random.Next(6, 12);
                objects[objects.Length - 1].curDir = new Vector2(random.Next(-10,10),random.Next(-10,10));
                objects[objects.Length - 1].pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
            if(Keyboard.GetState().IsKeyUp(Keys.Q))
            {
                spawnKeyPressed = false;
            }
        }

        public Vector2 CalculateAcceleration(Vector2 point, Object ignore = null)
        {

            Vector2 acceleration = Vector2.Zero;
            for (int i = 0; i < objects.Length; i++)
            {
                //very important if statement to check if the object is the same as its self
                //beacuse then that messes up the dist cal as you cannot divide 0 the dist
                //to itself is always 0
                if (objects[i] != ignore && objects[i].enabled)
                {
                    float dst = Vector2.Distance(objects[i].pos, point); //(float)Math.Sqrt(Vector2.Dot(objects[i].pos, point));
                    Vector2 forceDir = Vector2.Normalize(objects[i].pos - point);
                    acceleration += forceDir * gravity * objects[i].mass / (dst * distScaleFactor);
                }
            }

            return acceleration;
        }

    }
}
