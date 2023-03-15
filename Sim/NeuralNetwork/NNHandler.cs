using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NeuralNetworkHandler
{

    int inputLayerAmount = 2;
    int hiddenLayerAmount = 3;
    int outputLayerAmount = 1;


    float[] inputLayers;
    float[] inHidWeights;
    float[] inputBiases;
    float[] hiddenLayers;
    float[] hidOutWeights;
    float[] hiddenBiases;
    float[] outputLayers;
    float[] outputBiases;

    const float euler = 2.71828f;

    int visualX;
    int visualScaleX = 10;
    int visualY;
    int visualScaleY = 10;
    Color[] colors;

    public void InitNeuralNetwork(GraphicsDeviceManager graphics)
    {
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
            }
        }

        //calculate the weights connecting hidden to output layer (hidden to hidden layer not set up yet)
        for (int h = 0; h < hiddenLayerAmount; h++)
        {
            for (int w = 0; w < outputLayerAmount; w++)
            {
                hidOutWeights[w + outputLayerAmount * h] = new Random().NextSingle();
            }
        }

    }

    public void RunNerualNetwork()
    {
        for (int x = 0; x < visualX; x++)
        {
            for (int y = 0; y < visualY; y++)
            {
                inputLayers[0] = x;
                inputLayers[1] = y;

                //calculate the value for the hidden layer
                for (int h = 0; h < hiddenLayerAmount; h++)
                {
                    float weightSum = 0;
                    for (int i = 0; i < inputLayerAmount; i++)
                    {  
                        weightSum += CalculateOutput(inputLayers[i], inHidWeights[i], 0f);
                    }
                    weightSum = CalculateSigmoid(weightSum);
                    hiddenLayers[h] = weightSum;
                }

                //calculate the value for the output layer
                for (int o = 0; o < outputLayerAmount; o++)
                {
                    float weightSum = 0;
                    for (int h = 0; h < hiddenLayerAmount; h++)
                    {  
                        weightSum += CalculateOutput(hiddenLayers[h], hidOutWeights[h], 0f);
                    }
                    weightSum = CalculateSigmoid(weightSum);
                    outputLayers[o] = weightSum;
                }
                outputLayers[0] *= 255;
                colors[y + visualY * x] = new Color((int)outputLayers[0],(int)outputLayers[0],(int)outputLayers[0]);
            }
        }
    }

    //input is the input node value, weight is the weight value connecting the input node to the output node
    public float CalculateOutput(float input, float weight, float bias)
    {
        return input * weight + bias;
    }

    public float CalculateSigmoid(float input)
    {
        return 1 / (1 + (float)Math.Exp(-input));
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
}