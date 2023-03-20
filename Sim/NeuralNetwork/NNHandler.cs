using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting{
public class NeuralNetworkHandler
{

    int inputLayerAmount = 2;
    int hiddenLayerAmount = 3;
    int outputLayerAmount = 1;
    float inputScaleX = 5;
    float inputScaleY = 5;


    float[] inputLayers;
    float[] inHidWeights;
    float inputBias;
    float[] hiddenLayers;
    float[] hidOutWeights;
    float hiddenBias;
    float[] outputLayers;

    const float euler = 2.71828f;

    int visualX;
    int visualScaleX = 10;
    int visualY;
    int visualScaleY = 10;
    Color[] colors;
    int saveCount = 1;

    NeuralNetworkForwardPropogation neuralNetworkFP = new NeuralNetworkForwardPropogation();
    bool pressedQ = false;
    bool pressedS = false;
    bool pressedX = false;

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
        inputLayers = new float[inputLayerAmount];
        hiddenLayers = new float[hiddenLayerAmount];
        outputLayers = new float[outputLayerAmount];
        inHidWeights = new float[inputLayerAmount * hiddenLayerAmount];
        hidOutWeights = new float[hiddenLayerAmount * outputLayerAmount];

        visualX = graphics.PreferredBackBufferWidth / visualScaleX;
        visualY = graphics.PreferredBackBufferHeight / visualScaleY;
        colors = new Color[visualX * visualY];

        //calculate the weights connecting input to hidden layer
        for (int i = 0; i < inputLayerAmount; i++)
        {
            for (int h = 0; h < hiddenLayerAmount; h++)
            {
                inHidWeights[h + hiddenLayerAmount * i] = new Random().NextSingle();
                inputBias = -5.5f;
            }
        }

        //calculate the weights connecting hidden to output layer (hidden to hidden layer not set up yet)
        for (int h = 0; h < hiddenLayerAmount; h++)
        {
            for (int w = 0; w < outputLayerAmount; w++)
            {
                hidOutWeights[w + outputLayerAmount * h] = new Random().NextSingle();
                hiddenBias = -0.5f;
            }
        }

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
                inputLayers[0] = x / inputScaleX;
                inputLayers[1] = y / inputScaleY;

                hiddenLayers = neuralNetworkFP.CalculateOutput(inputLayerAmount, hiddenLayerAmount, inputLayers, inHidWeights, inputBias, 0.5f);
                outputLayers = neuralNetworkFP.CalculateOutput(hiddenLayerAmount, outputLayerAmount, hiddenLayers, hidOutWeights, hiddenBias, 0.5f);
                outputLayers[0] *= 255;
                colors[y + visualY * x] = new Color((int)outputLayers[0],(int)outputLayers[0],(int)outputLayers[0]);
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
        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++) 
            {
                spriteBatch.Begin();
                spriteBatch.Draw(pixel, new Vector2(x * visualScaleX,y * visualScaleY), colors[y + visualY * x]);
                spriteBatch.End();
            }
        }
    }

    public void ApplySaveData()
    {
        Saver saver = new Saver();
        saver.CreateFolder();
        SaverDataToSet.nerualNetworkSettings.hidOutWeights = hidOutWeights;
        SaverDataToSet.nerualNetworkSettings.inHidWeights = inHidWeights;
        SaverDataToSet.nerualNetworkSettings.hiddenBias = hiddenBias; 
        SaverDataToSet.nerualNetworkSettings.inputBias = inputBias;
        SaverDataToSet.nerualNetworkSettings.saveCount = saveCount;
        saver.SaveNeuralNetworkSimCount();
        saver.SaveNeuralNetworkSimSettings();
    }

    public void ReadSaveData()
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        saver.ReadNerualNetworkSimSettings();
        hidOutWeights = SaverDataToSet.nerualNetworkSettings.hidOutWeights;
        inHidWeights = SaverDataToSet.nerualNetworkSettings.inHidWeights;
        hiddenBias = SaverDataToSet.nerualNetworkSettings.hiddenBias;
        inputBias = SaverDataToSet.nerualNetworkSettings.inputBias;
        saveCount = SaverDataToSet.nerualNetworkSettings.saveCount;
        RunNerualNetwork();
    }
}
}