using System;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Linq;

public class LDA_MI : MonoBehaviour
{
    public string csvPath = "Assets/Data/lda_model_2000.csv";  // CSV�̃p�X
    private LDAClassifier lda;
    [SerializeField] private Bug bug; // Unity Editor でアタッチ
 
    // csp�s���������
    Matrix<double> csp = DenseMatrix.OfArray(new double[,]
    {
        { 1.77034646e-02, -4.54987431e-03, 3.52249816e-02, 1.37848062e-02, -3.35305598e-02, 2.38820740e-02, 4.12253377e-02, -2.35375941e-02 },
        { -2.60318411e-02, -3.92003525e-03, -1.64966211e-02, -2.92947319e-03, 8.90386091e-04, -2.77709159e-02, 7.86264573e-03, 1.19186491e-01 },
        { 5.25788847e-02, 2.18405245e-02, 5.15488746e-02, -3.89820280e-02, 6.15755414e-02, 1.62845318e-02, -1.10134530e-01, -4.45499330e-02 },
        { -8.46505208e-02, 4.46435553e-03, -1.93677542e-02, -5.32375751e-03, 2.11694155e-02, -3.69334607e-03, 8.26484868e-03, -7.91842954e-05 },
        { 2.23866022e-03, -6.91703068e-02, -7.86687786e-02, 6.15594995e-02, 1.58066302e-02, 1.60909489e-02, 2.81153877e-02, -9.32320915e-03 },
        { 1.18574398e-02, 5.05740717e-03, -1.83987410e-02, -5.69804045e-02, -7.22815588e-02, 5.28075135e-02, -1.92991497e-02, -6.03109402e-02 },
        { 2.65453298e-02, 4.79806727e-02, 3.99135696e-02, 1.27598916e-02, 2.02013149e-02, -6.46713047e-02, 6.93584307e-02, 3.30448051e-02 },
        { 2.06342955e-03, -8.80279875e-03, 2.82200158e-03, -6.18223614e-03, -1.83287418e-03, -2.30489489e-03, -9.45561859e-03, -3.45863023e-03 }
    });

    public Vector<double> ExtractCSPFeaturesFromBufferedData(float[,] bufferedData, Matrix<double> cspFilters)
    {
        int numFrames = bufferedData.GetLength(0); // = 500
        int numChannels = bufferedData.GetLength(1); // = 8

        // �]�u���� [channels x samples] �ɕϊ�
        double[,] eegTransposed = new double[numChannels, numFrames];
        for (int i = 0; i < numFrames; i++)
            for (int j = 0; j < numChannels; j++)
                eegTransposed[j, i] = bufferedData[i, j];

        var eegMatrix = Matrix<double>.Build.DenseOfArray(eegTransposed);

        // CSP�K�p
        var projected = cspFilters * eegMatrix;

        // �e�s�iCSP�����j�� log-variance �������o
        var features = Vector<double>.Build.Dense(projected.RowCount);
        for (int i = 0; i < projected.RowCount; i++)
        {
            var row = projected.Row(i);
            features[i] = Math.Log(CalculateVariance(row) + 1e-6); // ���U���v�Z����log�ϊ�
        }

        return features;
    }

    // �x�N�g���̕��U���v�Z���郁�\�b�h
    public double CalculateVariance(Vector<double> vector)
    {
        var data = vector.ToArray();  // double[] �ɕϊ�
        double mean = data.Average(); // System.Linq ���g�p
        double variance = data.Select(x => Math.Pow(x - mean, 2)).Average();
        return variance;
    }


    public void LDA_Classify()
    {
        lda = new LDAClassifier();
        lda.LoadFromCSV(csvPath);

        // ��FCSP���璊�o����2�̓�����
        Vector<double> feature = ExtractCSPFeaturesFromBufferedData(bug.GetBufferedData(), csp);
        int predictedClass = lda.PredictLabel(feature);

        Debug.Log("Predicted Class: " + predictedClass);
        Debug.Log(feature.Count);
    }
}
