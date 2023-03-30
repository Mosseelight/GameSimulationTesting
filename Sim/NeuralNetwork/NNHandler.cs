using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting{
public class NeuralNetworkHandler
{

    int inputNodeAmount = 2;
    //no weights connecting hidden to hidden
    int hiddenLayerAmount = 1;
    int hiddenNodeAmount = 3;
    int outputNodeAmount = 1;
    int[][] nodeAmounts;
    float inputScaleX = 5;
    float inputScaleY = 5;


    //use first index of 0 because only one column of inputs
    float[][] inputValues;
    float inputBias;
    //row be the values and colunm be the index of which layer it is
    float[][] hiddenValues;
    float hiddenBias;
    float[][] outputValues;
    float[][] values;
    float[][] weights;

    const float euler = 2.71828f;

    int visualX;
    int visualScaleX = 5;
    int visualY;
    int visualScaleY = 5;
    Color[] colors;
    int saveCount = 1;

    NeuralNetworkForwardPropogation neuralNetworkFP = new NeuralNetworkForwardPropogation();
    bool pressedQ = false;
    bool pressedS = false;
    bool pressedX = false;
    bool pressedW = false;

    [Serializable]public class NerualNetworkSettings
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
        saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;
        inputValues = new float[1][];
        nodeAmounts = new int[inputNodeAmount + hiddenNodeAmount + outputNodeAmount][];
        values = new float[1 + hiddenLayerAmount][];
        weights = new float[2 + hiddenLayerAmount][];
        inputValues[0] = new float[inputNodeAmount];
        hiddenValues = new float[hiddenLayerAmount][];
        for (int i = 0; i < hiddenLayerAmount; i++)
        {
            hiddenValues[i] = new float[hiddenNodeAmount];
        }
        for (int l = 0; l < 2 + hiddenLayerAmount; l++)
        {
            if(l == 0)
            {
                nodeAmounts[l] = new int[inputNodeAmount];
                weights[l] = new float[inputNodeAmount * hiddenNodeAmount];
            }
            if(l > 0 && l < 1 + hiddenLayerAmount)
            {
                nodeAmounts[l] = new int[hiddenNodeAmount];
                weights[l] = new float[hiddenNodeAmount * hiddenNodeAmount];
            }
            if(l == 1 + hiddenLayerAmount)
            {
                nodeAmounts[l] = new int[outputNodeAmount];
                weights[l] = new float[hiddenNodeAmount * outputNodeAmount];
            }
        }
        outputValues = new float[1][];
        outputValues[0] = new float[outputNodeAmount];

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
        
        //for multiple layers have an index for which layer it is at
        //and use the 2 for loops for input and output, using the index
        //to find the amount of times the for loops have to run
        //use jagged arrays for the values and bias and check to see if work
        //when the layer index increses it should increse the index for the jagged arrays so that they switch to a different layer

        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++)
            {
                inputValues[0][0] = x / inputScaleX;
                inputValues[0][1] = y / inputScaleY;

                for (int l = 1; l < 2 + hiddenLayerAmount; l++)
                {
                    values[l] = neuralNetworkFP.CalculateOutput(nodeAmounts[l].Length, nodeAmounts[l].Length, l, 1, inputValues, weights, inputBias, 0.5f)[l];
                    if(l == 1 + hiddenLayerAmount)
                    {
                        values[l][0] *= 255;
                        colors[y + visualY * x] = new Color((int)outputValues[0][0],(int)outputValues[0][0],(int)outputValues[0][0]);
                    }
                }
            }
        }
    }

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
        SaverDataToSet.nerualNetworkSettings.weights = weights;
        SaverDataToSet.nerualNetworkSettings.hiddenBias = hiddenBias; 
        SaverDataToSet.nerualNetworkSettings.inputBias = inputBias;
        SaverDataToSet.nerualNetworkSettings.saveCount = saveCount;
        saver.SaveNeuralNetworkSimSettingsJSON();
        saver.SaveNeuralNetworkSimCount();
        saver.SaveNeuralNetworkSimSettings();
    }

    public void ReadSaveData()
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        saver.ReadNerualNetworkSimSettings();
        weights = SaverDataToSet.nerualNetworkSettings.weights;
        hiddenBias = SaverDataToSet.nerualNetworkSettings.hiddenBias;
        inputBias = SaverDataToSet.nerualNetworkSettings.inputBias;
        saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;
        RunNerualNetwork();
    }
}
}