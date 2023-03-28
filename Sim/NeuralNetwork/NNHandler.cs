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
    float inputScaleX = 5;
    float inputScaleY = 5;


    //use first index of 0 because only one column of inputs
    float[][] inputValues;
    float inputBias;
    //row be the values and colunm be the index of which layer it is
    float[][] hiddenValues;
    float hiddenBias;
    float[][] outputValues;

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
        public float[] inHidWeights {get; set;}
        public float[] hidOutWeights {get; set;}
    }

    public void InitNeuralNetwork(GraphicsDeviceManager graphics)
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;
        inputValues = new float[1][];
        inputValues[0] = new float[inputNodeAmount];
        hiddenValues = new float[hiddenLayerAmount][];
        for (int i = 0; i < hiddenLayerAmount; i++)
        {
            hiddenValues[i] = new float[hiddenNodeAmount];
        }
        outputValues = new float[1][];
        outputValues[0] = new float[outputNodeAmount];

        visualX = graphics.PreferredBackBufferWidth / visualScaleX;
        visualY = graphics.PreferredBackBufferHeight / visualScaleY;
        colors = new Color[visualX * visualY];

    }

    public void RunNerualNetwork()
    {
        //for multiple layers have an index for which layer it is at
        //and use the 2 for loops for input and output, using the index
        //to find the amount of times the for loops have to run

        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++)
            {
                inputValues[0][0] = x / inputScaleX;
                inputValues[0][1] = y / inputScaleY;

                hiddenValues = neuralNetworkFP.CalculateOutput(inputNodeAmount, hiddenNodeAmount, 1, inputValues, inputBias, 0.5f);
                outputValues = neuralNetworkFP.CalculateOutput(hiddenNodeAmount, outputNodeAmount, hiddenLayerAmount, hiddenValues, hiddenBias, 0.5f);
                outputValues[0][0] *= 255;
                colors[y + visualY * x] = new Color((int)outputValues[0][0],(int)outputValues[0][0],(int)outputValues[0][0]);
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
        hiddenBias = SaverDataToSet.nerualNetworkSettings.hiddenBias;
        inputBias = SaverDataToSet.nerualNetworkSettings.inputBias;
        saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;
        RunNerualNetwork();
    }
}
}