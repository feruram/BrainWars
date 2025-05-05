
using System.IO;
using System.Linq;
using UnityEngine;

public static class ModelManager
{
    private static string filePath = "Assets/lda_model.csv"; // `Assets/` フォルダ内に保存

    public static void SaveModel()
    {
        double[] weights = OnlineLDAClassifier.GetWeights();
        double bias = OnlineLDAClassifier.GetBias();

        if (weights == null || weights.Length == 0)
        {
            Debug.LogWarning("保存できるモデルがありません！");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(bias);
            writer.WriteLine(string.Join(",", weights));
        }

        Debug.Log($"モデルを CSV に保存しました: {filePath}");
    }

    public static void LoadModel()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            double bias = double.Parse(lines[0]);
            double[] weights = lines[1].Split(',').Select(double.Parse).ToArray();

            OnlineLDAClassifier.SetWeights(weights);
            OnlineLDAClassifier.SetBias(bias);

            Debug.Log($"モデルを CSV からロードしました: {filePath}");
        }
        else
        {
            Debug.LogWarning($"モデルファイルが見つかりません！: {filePath}");
        }
    }
}
