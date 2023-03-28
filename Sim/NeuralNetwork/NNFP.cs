using System;

public class NeuralNetworkForwardPropogation
{
    public float[][] CalculateOutput(int inputNum, int outputNum, int layerValue, float[][] inputValue, float bias, float biasWeight)
    {
        float[][] weightSums = new float[layerValue][];
        float[] weights = new float[inputNum * outputNum];
        for (int i = 0; i < inputNum; i++)
        {
            for (int o = 0; o < outputNum; o++)
            {
                weights[o + outputNum * i] = RandomNumber(-1f,1f);
                bias = RandomNumber(-10f,10f);
            }
        }
        for (int i = 0; i < layerValue; i++)
        {
            weightSums[i] = new float[inputNum * outputNum];
        }
        int weightIndex = 0;
        for (int l = 0; l < layerValue; l++)
        {
            for (int o = 0; o < outputNum; o++)
            {
                float weightSum = 0;
                for (int i = 0; i < inputNum; i++)
                {
                    weightIndex = i + inputNum * o;
                    weightSum += CalculateValue(inputValue[0][i], weights[weightIndex]);
                }
                weightSum += CalculateValue(bias, biasWeight);
                weightSum = CalculateSigmoidShrink(weightSum);
                weightSums[layerValue - 1][o] = weightSum;
            }
        }
        weights = null;
        return weightSums;
    }

    static float RandomNumber(float min, float max)
    {
        Random random = new Random();
        float val = (random.NextSingle() * (max - min) + min);
        return val;
    }

    float CalculateValue(float input, float weight)
    {
        return input * weight;
    }

    float CalculateArcTan(float input)
    {
        return (float)Math.Atan(input);
    }

    float CalculateSigmoid(float input)
    {
        return 1 / (1 + (float)Math.Exp(-input));
    }

    float CalculateLinear(float input)
    {
        return input;
    }
    
    float CalculateStep(float input)
    {
        if(input < 0)
        {
            return 0;
        }
        else 
        {
            return 1;
        }
    }

    float CalculateReLU(float input)
    {
        return Math.Max(0, input);
    }

    float CalculateSoftPlus(float input)
    {
        return (float)Math.Log(1+Math.Exp(input));
    }

    float CalculateSigmoidShrink(float input)
    {
        return input / (1 + (float)Math.Exp(-input));
    }
}