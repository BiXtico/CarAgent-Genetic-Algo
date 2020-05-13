 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using System;

using Random = UnityEngine.Random;

public class NNetwork : MonoBehaviour
{
    public Matrix<float> input = Matrix<float>.Build.Dense(1, 3);
    public Matrix<float> output = Matrix<float>.Build.Dense(1, 2);

    public List<Matrix<float>> weights = new List<Matrix<float>>();
    public List<Matrix<float>> innerlayers = new List<Matrix<float>>();
    public List<float> biases = new List<float>();

    public float performance;
    
    public void Initialise (int hiddenLayerCount, int hiddenNeuronCount)
    {

        input.Clear();
        innerlayers.Clear();
        output.Clear();
        weights.Clear();
        biases.Clear();

        for (int i = 0; i <= hiddenLayerCount ; i++)
        {

            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);

            innerlayers.Add(f);

            biases.Add(Random.Range(-1f, 1f));
            
            if (i == 0) 
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(3, hiddenNeuronCount);
                weights.Add(inputToH1); 
            }

            Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            weights.Add(HiddenToHidden);

        }
        Matrix<float> outputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 2);
        weights.Add(outputWeight);
        biases.Add(Random.Range(-1f, 1f));

        RandomiseWeights();

    }

    public void RandomiseWeights() 
    {

        for (int i = 0; i < weights.Count; i++)
        {

            for (int x = 0; x < weights[i].RowCount; x++)
            {

                for (int y = 0; y < weights[i].ColumnCount; y++)
                {

                    weights[i][x, y] = Random.Range(-1f, 1f);

                }

            }

        }

    }

    public NNetwork InitialiseCopy(int hiddenLayerCount, int hiddenNeuronCoint)
    {
        NNetwork n = new NNetwork();
        List<Matrix<float>> newWeights = new List<Matrix<float>>();
        for (int i = 0; i < this.weights.Count; i++)
        {
            Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);
            for (int x = 0; x < currentWeight.RowCount; x++)
            {
                for (int y = 0; y < currentWeight.ColumnCount; y++)
                {
                    currentWeight[x, y] = weights[i][x, y];
                }
            }
            newWeights.Add(currentWeight);
        }
        List<float> newBisaes = new List<float>();
        newBisaes.AddRange(biases);

        n.weights = newWeights;
        n.biases = newBisaes;

        n.InitialiseHidden(hiddenLayerCount,hiddenNeuronCoint);
        return n;
    }

    public void InitialiseHidden(int hiddenLayerCount, int hiddenNeuronCount)
    {
        input.Clear();
        innerlayers.Clear();
        output.Clear();

        for (int i=0; i < hiddenLayerCount + 1; i++)
        {
            Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenLayerCount);
            innerlayers.Add(newHiddenLayer);
        }
    }
  
    public (float, float) RunNetwork (float a, float b, float c)
    {
        input[0, 0] = a;
        input[0, 1] = b;
        input[0, 2] = c;
        
        input = input.PointwiseTanh();

        innerlayers[0] = ((input * weights[0]) + biases[0]).PointwiseTanh();
        for (int i = 1; i < innerlayers.Count; i++)
        {
            innerlayers[i] = ((innerlayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        output = ((innerlayers[innerlayers.Count-1]*weights[weights.Count-1])+biases[biases.Count-1]).PointwiseTanh();

        return (Sigmoid(output[0,0]), (float)Math.Tanh(output[0,1]));
    }

    private float Sigmoid (float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }

}
