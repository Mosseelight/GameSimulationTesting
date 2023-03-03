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
        int aftObjectsCount = 0;

        //settings
        int numObjs = 200;
        float gravity = 1.5f;
        float collsionPushFactor = -0.5f;
        bool haveSun = false;
        bool randomPos = true;
        int posXChoose = 950;
        int posYChoose = 500;
        int posTolerence = 50;
        float minCollideDist = 10f;
        float minSeperationDist = 0.5f;
        float maxSeperationDist = 2000f;
        float overlapCorrectionDist = 1f;

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
            public float minSeperationDist { get; set; }
            public float maxSeperationDist { get; set; }
            public float overlapCorrectionDist { get; set; }
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
            SaverDataToSet.objectSimSettingsSet.minSeperationDist = minSeperationDist;
            SaverDataToSet.objectSimSettingsSet.maxSeperationDist = maxSeperationDist;
            SaverDataToSet.objectSimSettingsSet.overlapCorrectionDist = overlapCorrectionDist;
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
                    objects[i].mass = 100;
                    int X = 900;
                    int Y = 500;
                    objects[i].pos = new Vector2(X, Y);
                }
                else
                {
                    objects[i].mass = random.Next(1, 5);
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
        }

        public void UpRunSimulation()
        {
            for(int i = 0; i < objectsList.Count; i++)
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
                    checkforoverlap = true;
                }
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

                for (int b = 0; b < objectsList.Count; b++)
                {
                    //b != i checks if its self is the same as its self so it does not give a dist
                    //of 0
                    if (b != i && Vector2.Distance(objects[i].pos, objects[b].pos) < minCollideDist && objects[b].enabled && objects[i].enabled)
                    {
                        objects[b].enabled = false;
                        objects[i].enabled = false;
                        objectsList.Add(new Object());
                        objects = objectsList.ToArray();
                        objects[aftObjectsCount + numObjs].enabled = true;
                        objects[aftObjectsCount + numObjs].mass = objects[b].mass + objects[i].mass;
                        objects[aftObjectsCount + numObjs].curDir = objects[b].curDir - objects[i].curDir;
                        objects[aftObjectsCount + numObjs].pos = Vector2.Lerp(objects[b].pos, objects[i].pos, 0.5f);
                        aftObjectsCount++;
                    }
                }

            }
        }

        public void DrawSim(Texture2D circle, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont font)
        {
            Vector2 textMiddlePos = font.MeasureString("Gravity " + gravity) / 2;
            for (int i = 0; i < objectsList.Count; i++)
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
                SaverDataToSet.objectSimSettingsSet.minSeperationDist = minSeperationDist;
                SaverDataToSet.objectSimSettingsSet.maxSeperationDist = maxSeperationDist;
                SaverDataToSet.objectSimSettingsSet.overlapCorrectionDist = overlapCorrectionDist;
                saver.SaveSimSettings();
                Console.WriteLine("Saved");
            }
        }

        public Vector2 CalculateAcceleration(Vector2 point, Object ignore = null)
        {

            Vector2 acceleration = Vector2.Zero;
            for (int i = 0; i < objectsList.Count; i++)
            {
                //very important if statement to check if the object is the same as its self
                //beacuse then that messes up the dist cal as you cannot divide 0 the dist
                //to itself is always 0
                if (objects[i] != ignore && objects[i].enabled)
                {
                    float dst = Vector2.Distance(objects[i].pos, point); //(float)Math.Sqrt(Vector2.Dot(objects[i].pos, point));
                    Vector2 forceDir = Vector2.Normalize(objects[i].pos - point);
                    acceleration += forceDir * gravity * objects[i].mass / Math.Clamp(dst, minSeperationDist, maxSeperationDist);
                }
            }

            return acceleration;
        }

    }
}
