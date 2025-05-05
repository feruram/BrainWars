
using System.IO;
using System.Linq;
using UnityEngine;

public static class ModelManager
{
    private static string filePath = "Assets/lda_model.csv"; // `Assets/` �t�H���_���ɕۑ�

    public static void SaveModel()
    {
        double[] weights = OnlineLDAClassifier.GetWeights();
        double bias = OnlineLDAClassifier.GetBias();

        if (weights == null || weights.Length == 0)
        {
            Debug.LogWarning("�ۑ��ł��郂�f��������܂���I");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(bias);
            writer.WriteLine(string.Join(",", weights));
        }

        Debug.Log($"���f���� CSV �ɕۑ����܂���: {filePath}");
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

            Debug.Log($"���f���� CSV ���烍�[�h���܂���: {filePath}");
        }
        else
        {
            Debug.LogWarning($"���f���t�@�C����������܂���I: {filePath}");
        }
    }
}
