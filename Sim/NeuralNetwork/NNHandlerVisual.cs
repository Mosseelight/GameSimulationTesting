using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting{
public class NeuralNetworkHandlerVisual
{

    int inputNodeAmount = 2;
    int hiddenLayerAmount = 4;
    int hiddenNodeAmount = 8;
    int outputNodeAmount = 1;
    int[][] nodeAmounts;
    float inputScaleX = 5;
    float inputScaleY = 5;


    float inputBias;
    float hiddenBias;
    float[][] values;
    float[][] weights;

    int visualX;
    int visualScaleX = 7;
    int visualY;
    int visualScaleY = 7;
    Color[] colors;
    int saveCount = 1;
    int xyCount = 0;

    NeuralNetworkForwardPropogation neuralNetworkFP = new NeuralNetworkForwardPropogation();
    bool pressedQ = false;
    bool pressedS = false;
    bool pressedX = false;
    bool pressedW = false;
    bool pressedE = false;

    [Serializable]public class NerualNetworkVisualSettings
    {
        public int saveCount {get; set;}
        public float inputBias {get; set;}
        public float hiddenBias {get; set;}
        public float[][] weights {get; set;}
    }

    public void InitNeuralNetwork(GraphicsDeviceManager graphics)
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        saveCount = SaverDataToSet.nerualNetworkVisualSettings.saveCount;
        nodeAmounts = new int[inputNodeAmount + hiddenNodeAmount + outputNodeAmount][];
        values = new float[2 + hiddenLayerAmount][];
        weights = new float[2 + hiddenLayerAmount][];
        for (int l = 0; l < 2 + hiddenLayerAmount; l++)
        {
            if(l == 0)
            {
                nodeAmounts[l] = new int[inputNodeAmount];
                values[l] = new float[inputNodeAmount];
                weights[l] = new float[inputNodeAmount * hiddenNodeAmount];
            }
            if(l > 0 && l < 1 + hiddenLayerAmount)
            {
                nodeAmounts[l] = new int[hiddenNodeAmount];
                values[l] = new float[hiddenNodeAmount];
                weights[l] = new float[hiddenNodeAmount * hiddenNodeAmount];
            }
            if(l == 1 + hiddenLayerAmount)
            {
                nodeAmounts[l] = new int[outputNodeAmount];
                values[l] = new float[outputNodeAmount];
                weights[l] = new float[hiddenNodeAmount * outputNodeAmount];
            }
        }

        visualX = graphics.PreferredBackBufferWidth / visualScaleX;
        visualY = graphics.PreferredBackBufferHeight / visualScaleY;
        colors = new Color[visualX * visualY];

        RandomizeWeights();
    }

    //do similar way to forwardpropogation to assign weights random value
    public void RandomizeWeights()
    {
        //calculate the weights connecting input to hidden layer
        for (int i = 0; i < inputNodeAmount; i++)
        {
            for (int h = 0; h < hiddenNodeAmount; h++)
            {
                weights[0][h + hiddenNodeAmount * i] =  RandomNumber(-1f,1f);
                inputBias = RandomNumber(-1f,1f);
            }
        }

        //calculate the weights connecting hidden to hidden layer
        for (int l = 0; l < 2 + hiddenLayerAmount; l++)
        {
            for (int h = 0; h < hiddenNodeAmount; h++)
            {
                for (int w = 0; w < hiddenNodeAmount; w++)
                {
                    if(l != 1 + hiddenLayerAmount && l != 0)
                    {
                        weights[l][w + hiddenNodeAmount * h] = RandomNumber(-1f,1f);
                        hiddenBias = RandomNumber(-1f,1f);
                    }
                }
            }
        }

        //calculate the weights connecting hidden to output layer
        for (int o = 0; o < outputNodeAmount; o++)
        {
            for (int h = 0; h < hiddenNodeAmount; h++)
            {
                weights[1 + hiddenLayerAmount][h + hiddenNodeAmount * o] = RandomNumber(-1f,1f);
                hiddenBias = RandomNumber(-1f,1f);
            }
        }
    }
    static float RandomNumber(float min, float max)
    {
        Random random = new Random();
        float val = (random.NextSingle() * (max - min) + min);
        return val;
    }

    public void RunNerualNetwork()
    {
        for (int x = -visualX / 2; x < visualX / 2; x++)
        {
            for (int y = -visualY / 2; y < visualY / 2; y++)
            {
                values[0][0] = x / inputScaleX;
                values[0][1] = y / inputScaleY;

                for (int l = 0; l < 2 + hiddenLayerAmount; l++)
                {
                    if(l == 1 + hiddenLayerAmount)
                    {
                        values[l][0] *= 255;
                        colors[xyCount] = new Color((int)values[l][0],(int)values[l][0],(int)values[l][0]);
                        /*if(values[l][0] < 85)
                        {
                            colors[xyCount] = new Color(0,0,(int)values[l][0] * 2);
                        }
                        if(values[l][0] > 85 && values[l][0] < 170)
                        {
                            colors[xyCount] = new Color(0,(int)values[l][0] * 2,0);
                        }
                        if(values[l][0] > 170)
                        {
                            colors[xyCount] = new Color((int)values[l][0] * 2,0,0);
                        }*/
                    }
                    else
                    {
                        values[l + 1] = neuralNetworkFP.CalculateOutput(nodeAmounts[l].Length, nodeAmounts[l + 1].Length, l, values, weights, inputBias, 0.5f)[l];
                    }
                }
                xyCount++;
            }
        }
        xyCount = 0;
    }

    /*
    public void TrainNueralNetwork()
    {
        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++)
            {
                values[0][0] = x / inputScaleX;
                values[0][1] = y / inputScaleY;

                for (int l = 0; l < 2 + hiddenLayerAmount; l++)
                {
                    if(l == 1 + hiddenLayerAmount)
                    {
                        //process the value through BP


                        values[l][0] *= 255;
                        if(values[l][0] < 85)
                        {
                            colors[y + visualY * x] = new Color(0f,0f,values[l][0]);
                        }
                        if(values[l][0] > 85 && values[l][0] < 170)
                        {
                            colors[y + visualY * x] = new Color(0f,(int)values[l][0],0f);
                        }
                        if(values[l][0] > 170)
                        {
                            colors[y + visualY * x] = new Color((int)values[l][0],0f,0f);
                        }
                    }
                    else
                    {
                        values[l + 1] = neuralNetworkFP.CalculateOutput(nodeAmounts[l].Length, nodeAmounts[l + 1].Length, l, values, weights, inputBias, 0.5f)[l];
                    }
                }
            }
        }
    }
    */

    public void HandleInput()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Q) && !pressedQ)
        {
            pressedQ = true;
            RunNerualNetwork(); 
        }
        if(Keyboard.GetState().IsKeyUp(Keys.Q))
        {
            pressedQ = false;
        }
        //save data
        if (Keyboard.GetState().IsKeyDown(Keys.S) && !pressedS)
        {
            pressedS = true;
            saveCount++;
            ApplySaveData();
        }
        if(Keyboard.GetState().IsKeyUp(Keys.S))
        {
            pressedS = false;
        }
        //load data
        if (Keyboard.GetState().IsKeyDown(Keys.X) && !pressedX)
        {
            pressedX = true;
            ReadSaveData();
        }
        if(Keyboard.GetState().IsKeyUp(Keys.X))
        {
            pressedX = false;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.W) && !pressedW)
        {
            pressedW = true;
            RandomizeWeights();
        }
        if(Keyboard.GetState().IsKeyUp(Keys.W))
        {
            pressedW = false;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.E) && !pressedE)
        {
            pressedE = true;
            RandomizeWeights();
            RunNerualNetwork(); 
        }
        if(Keyboard.GetState().IsKeyUp(Keys.E))
        {
            pressedE = false;
        }
    }

    public void DrawPixels(Texture2D pixel, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
    {
        spriteBatch.Begin();
        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++) 
            {
                spriteBatch.Draw(pixel, new Vector2(x * visualScaleX,y * visualScaleY), colors[y + visualY * x]);
            }
        }
        spriteBatch.End();
    }

    public void ApplySaveData()
    {
        Saver saver = new Saver();
        saver.CreateFolder();
        SaverDataToSet.nerualNetworkVisualSettings.weights = weights;
        SaverDataToSet.nerualNetworkVisualSettings.hiddenBias = hiddenBias; 
        SaverDataToSet.nerualNetworkVisualSettings.inputBias = inputBias;
        SaverDataToSet.nerualNetworkVisualSettings.saveCount = saveCount;
        saver.SaveNeuralNetworkSimSettingsJSON();
        saver.SaveNeuralNetworkSimCount();
    }

    public void ReadSaveData()
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        weights = SaverDataToSet.nerualNetworkVisualSettings.weights;
        hiddenBias = SaverDataToSet.nerualNetworkVisualSettings.hiddenBias;
        inputBias = SaverDataToSet.nerualNetworkVisualSettings.inputBias;
        saveCount = SaverDataToSet.nerualNetworkVisualSettings.saveCount;
        RunNerualNetwork();
    }
}
}