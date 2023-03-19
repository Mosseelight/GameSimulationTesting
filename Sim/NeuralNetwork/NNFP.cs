using System;

public class NeuralNetworkForwardPropogation
{
    public float[] CalculateOutput(int inputNum, int outputNum, float[] inputValue, float[] weights, float bias, float biasWeight)
    {
        float[] weightSums = new float[inputNum * outputNum];
        int weightIndex = 0;
        for (int o = 0; o < outputNum; o++)
        {
            float weightSum = 0;
            for (int i = 0; i < inputNum; i++)
            {
                weightIndex = i + inputNum * o;
                weightSum += CalculateValue(inputValue[i], weights[weightIndex]);
            }
            weightSum += CalculateValue(bias, biasWeight);
            weightSum = CalculateSigmoid(weightSum);
            weightSums[o] = weightSum;
        }
        return weightSums;
    }

    float CalculateValue(float input, float weight)
    {
        return input * weight;
    }

    float CalculateSigmoid(float input)
    {
        return 1 / (1 + (float)Math.Exp(-input));
    }
}