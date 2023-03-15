using System;

public class NeuralNetworkHandler
{

    int inputLayerAmount = 2;
    int hiddenLayerAmount = 10;
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

    public void InitNeuralNetwork()
    {
        inputLayers = new float[inputLayerAmount];
        hiddenLayers = new float[hiddenLayerAmount];
        outputLayers = new float[outputLayerAmount];
        inHidWeights = new float[inputLayerAmount * hiddenLayerAmount];
        hidOutWeights = new float[hiddenLayerAmount * outputLayerAmount];

        //calculate the weights connecting input to hidden layer
        for (int i = 0; i < inputLayerAmount; i++)
        {
            for (int h = 0; h < hiddenLayerAmount; h++)
            {
                inHidWeights[h + hiddenLayerAmount * i] = 0.5f;
            }
        }

        //calculate the weights connecting hidden to output layer (hidden to hidden layer not set up yet)
        for (int h = 0; h < hiddenLayerAmount; h++)
        {
            for (int w = 0; w < outputLayerAmount; w++)
            {
                hidOutWeights[w + outputLayerAmount * h] = 0.5f;
            }
        }

        //-------TESTING------- give input layer a value
        for (int i = 0; i < inputLayerAmount; i++)
        {
            inputLayers[i] = 1;
        }

        //calculate the value for the hidden layer
        for (int h = 0; h < hiddenLayerAmount; h++)
        {
            float weightSum = 0;
            for (int i = 0; i < inputLayerAmount; i++)
            {  
                weightSum += CalculateOutput(inputLayers[i], inHidWeights[i], 0);
                Console.WriteLine("Input Node " + i + ": " + inputLayers[i] + " | Hidden Node: " + h);
            }
            weightSum = CalculateSigmoid(weightSum);
            Console.WriteLine("Hidden Node " + h + " Value: " + weightSum);
            hiddenLayers[h] = weightSum;
        }

        //calculate the value for the output layer
        for (int o = 0; o < outputLayerAmount; o++)
        {
            float weightSum = 0;
            for (int h = 0; h < hiddenLayerAmount; h++)
            {  
                weightSum += CalculateOutput(hiddenLayers[h], hidOutWeights[h], 0);
                Console.WriteLine("Hidden Node " + h + ": " + hiddenLayers[h] + " | Output Node: " + o);
            }
            weightSum = CalculateSigmoid(weightSum);
            Console.WriteLine("Output Node " + o + " Value: " + weightSum);
            outputLayers[o] = weightSum;
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
}