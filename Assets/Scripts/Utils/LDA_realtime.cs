using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class OnlineLDAClassifier
{
    private static double[] weights; // 線形分類器の重み
    private static double bias; // 分類のための閾値
    private static List<double[]> positiveBuffer = new List<double[]>(); // 正例データバッファ
    private static List<double[]> negativeBuffer = new List<double[]>(); // 負例データバッファ

    // **初期化メソッド**
    public static void Initialize(List<double[]> initialPos, List<double[]> initialNeg)
    {
        positiveBuffer.AddRange(initialPos);
        negativeBuffer.AddRange(initialNeg);
        Retrain();
        UnityEngine.Debug.Log("LDA分類器を初期キャリブレーションしました");
    }

    // **入力データに対する予測スコア**
    public static double Predict(double[] input)
    {
        return DotProduct(weights, input) - bias;
    }

    // **分類メソッド**
    public static bool Classify(double[] input)
    {
        return Predict(input) > 0;
    }

    // **ユーザーのフィードバックを受け取りバッファに追加**
    public static void UpdateWithFeedback(double[] input, bool isCorrectLabel)
    {
        if (isCorrectLabel)
            positiveBuffer.Add(input);
        else
            negativeBuffer.Add(input);

        if ((positiveBuffer.Count + negativeBuffer.Count) % 10 == 0)
        {
            Retrain();
        }
    }

    // **分類器の再学習**
    private static void Retrain()
    {
        if (positiveBuffer.Count == 0 || negativeBuffer.Count == 0)
        {
            UnityEngine.Debug.LogWarning("十分なデータがないため、再学習できません！");
            return;
        }

        int featureLength = positiveBuffer[0].Length;
        double[] meanPositive = MeanVector(positiveBuffer);
        double[] meanNegative = MeanVector(negativeBuffer);

        weights = meanPositive.Zip(meanNegative, (pos, neg) => pos - neg).ToArray();
        bias = DotProduct(weights, meanPositive.Zip(meanNegative, (pos, neg) => pos - neg).ToArray()) / 2;

        UnityEngine.Debug.Log("分類器を再学習しました");
    }

    // **2つのベクトルの内積**
    private static double DotProduct(double[] a, double[] b)
    {
        return a.Zip(b, (x, y) => x * y).Sum();
    }

    // **平均ベクトルの計算**
    private static double[] MeanVector(List<double[]> samples)
    {
        if (samples == null || samples.Count == 0)
        {
            UnityEngine.Debug.LogError("MeanVector: サンプルが空です。");
            return null;
        }

        int n = samples[0].Length;
        double[] mean = new double[n];

        foreach (var sample in samples)
        {
            if (sample.Length != n)
            {
                UnityEngine.Debug.LogError($"MeanVector: サンプルの長さが不一致です。期待: {n}, 実際: {sample.Length}");
                continue; // 長さが違うサンプルはスキップ
            }

            for (int i = 0; i < n; i++)
            {
                mean[i] += sample[i];
            }
        }

        for (int i = 0; i < n; i++)
            mean[i] /= samples.Count;

        return mean;
    }
    public static double[] GetWeights()
    {
        return weights;
    }

    public static double GetBias()
    {
        return bias;
    }

    public static void SetWeights(double[] newWeights)
    {
        weights = newWeights;
    }

    public static void SetBias(double newBias)
    {
        bias = newBias;
    }

}
