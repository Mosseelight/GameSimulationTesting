using System;

public class NeuralNetworkForwardPropogation
{
    public float[,] CalculateOutput(int inputNum, int outputNum, int layerAmount, float[,] inputValue, float[] weights, float bias, float biasWeight)
    {
        float[,] weightSums = new float[inputNum * outputNum, layerAmount];
        int weightIndex = 0;
        //for (int l = 0; l < layerAmount; l++)
        //{
            for (int o = 0; o < outputNum; o++)
            {
                float weightSum = 0;
                for (int i = 0; i < inputNum; i++)
                {
                    weightIndex = i + inputNum * o;
                    Console.WriteLine(inputValue[1,0]);
                    weightSum += CalculateValue(inputValue[1,0], weights[weightIndex]);
                }
                weightSum += CalculateValue(bias, biasWeight);
                weightSum = CalculateLinear(weightSum);
                weightSums[o,0] = weightSum;
            }
        //}
        return weightSums;
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