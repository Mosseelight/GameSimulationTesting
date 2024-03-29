using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameTesting{
public class NeuralNetworkHandlerWord
{

    int inputNodeAmount = 2;
    int hiddenLayerAmount = 4;
    int hiddenNodeAmount = 200;
    int[][] nodeAmounts;
    float inputScaleX = 5;
    float inputScaleY = 5;


    float inputBias;
    float hiddenBias;
    float[][] values;
    float[][] weights;

    int visualX;
    int visualScaleX = 5;
    int visualY;
    int visualScaleY = 5;
    Color[] colors;
    int saveCount = 1;


    int wordCount = 20;
    //put into a file so easeier to read
    string[] words = {"yes", "opalim", "te", "golim", "no", "bad", "good", 
                    "word", "poltics", "test", "ai", "neural", "network", 
                    "number", "simulation", "testing", "game", "liberal", "conservitive", 
                    "libertarian", "right", "left", "authortarian", 
                    "why", "what", "who", "when", "where", "percent", "one", "two", 
                    "three", "four", "five", "six", "seven", "eight", "nine", "ten", 
                    "rate", "spend", "help", "down", "drop", "half", "quarter", "full", 
                    "math", "science", "geography", "history", "affect", "effect",  
                    "internet", "transportation", "high", "low", "more", "less", 
                    "you", "they", "i", "me", "we", "their", "research", "note", "level",
                    "is", "eat", "make", "do", "evil", "smart", "am", "fake", "real", "for",
                    "paid", "money", "dollar", "rich", "red", "green", "blue", "yellow",
                    "purple", "cyan", "sky", "night", "dark", "light", "bright", "not", 
                    "will", "be", "handle", "name", "action", "or", "event", "have", "positive",
                    "negative", "power", "weak", "detail", "small", "big", "huge", "tiny",
                    "follow", "walk", "away", "close", "above", "below", "far", "very",
                    "detect", "find", "throw", "grab", "programming", "java", "c++",
                    "c#", "python", "water", "drink", "soil", "planet", "earth", "mars", 
                    "sun", "stone", "ground", "air", "breath", "human", "source", 
                    "against"
                    };
    string outputSentence;


    NeuralNetworkForwardPropogation neuralNetworkFP = new NeuralNetworkForwardPropogation();
    bool pressedQ = false;
    bool pressedS = false;
    bool pressedX = false;
    bool pressedW = false;
    bool pressedE = false;

    [Serializable]public class NerualNetworkWordSettings
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
        saveCount = SaverDataToSet.nerualNetworkWordSettings.saveCount;
        nodeAmounts = new int[inputNodeAmount + hiddenNodeAmount + words.Length][];
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
                nodeAmounts[l] = new int[words.Length];
                values[l] = new float[words.Length];
                weights[l] = new float[hiddenNodeAmount * words.Length];
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
        for (int o = 0; o < words.Length; o++)
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
        outputSentence = "";
        for (int i = 0; i < wordCount; i++)
        {
            values[0][0] = RandomNumber(-100,100);
            values[0][1] = RandomNumber(-100,100);

                for (int l = 0; l < 2 + hiddenLayerAmount; l++)
                {
                    if(l == 1 + hiddenLayerAmount)
                    {
                        float[] maxValues = new float[values[l].Length];
                        maxValues = values[l];
                        float max = maxValues.Max();
                        int index = maxValues.ToList().IndexOf(max);
                        outputSentence += " " + words[index];
                    }
                    else
                    {
                        values[l + 1] = neuralNetworkFP.CalculateOutput(nodeAmounts[l].Length, nodeAmounts[l + 1].Length, l, values, weights, inputBias, 0.5f)[l];
                    }
                }
        }
        Console.WriteLine(outputSentence);
    }

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
                        colors[y + visualY * x] = new Color((int)values[l][0],(int)values[l][0],(int)values[l][0]);
                    }
                    else
                    {
                        values[l + 1] = neuralNetworkFP.CalculateOutput(nodeAmounts[l].Length, nodeAmounts[l + 1].Length, l, values, weights, inputBias, 0.5f)[l];
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
        SaverDataToSet.nerualNetworkWordSettings.weights = weights;
        SaverDataToSet.nerualNetworkWordSettings.hiddenBias = hiddenBias; 
        SaverDataToSet.nerualNetworkWordSettings.inputBias = inputBias;
        SaverDataToSet.nerualNetworkWordSettings.saveCount = saveCount;
        saver.SaveNeuralNetworkSimSettingsJSON();
        saver.SaveNeuralNetworkSimCount();
    }

    public void ReadSaveData()
    {
        Saver saver = new Saver();
        saver.ReadNerualNetworkSimCount();
        weights = SaverDataToSet.nerualNetworkWordSettings.weights;
        hiddenBias = SaverDataToSet.nerualNetworkWordSettings.hiddenBias;
        inputBias = SaverDataToSet.nerualNetworkWordSettings.inputBias;
        saveCount = SaverDataToSet.nerualNetworkWordSettings.saveCount;
        RunNerualNetwork();
    }
}
}