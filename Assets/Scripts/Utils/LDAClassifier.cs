using UnityEngine;
using System.IO;
using System.Globalization;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class LDAClassifier
{
    public double[] weights;
    public double bias;
    
    public void LoadFromCSV(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length < 2) return;

        string[] values = lines[1].Split(',');
        weights = new double[2];
        weights[0] = double.Parse(values[0], CultureInfo.InvariantCulture); // W0
        weights[1] = double.Parse(values[1], CultureInfo.InvariantCulture); // W1
        bias = double.Parse(values[2], CultureInfo.InvariantCulture);       // b
    }

    public int PredictLabel(Vector<double> features)
    {
        double score = weights[0] * features[0] + weights[1] * features[7] + bias;
        return score > 0 ? 1 : 0;
    }

}
