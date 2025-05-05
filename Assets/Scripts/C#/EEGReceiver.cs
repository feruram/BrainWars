using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Reflection;

public class EEGReceiver : MonoBehaviour
{
    [SerializeField] private int ElectrodeIndex = 2;//何の電極を学習させるか[0～7]
    public bool predicted;
    private Queue<float[]> eegBuffer = new Queue<float[]>(); // 疑似 EEG バッファ
    private Queue<float[]> trainingBuffer = new Queue<float[]>(); // 学習用 EEG バッファ
    const int fs = 250;
    const float time_buffer = 0.5f;
    private const int BufferSize = (int)(fs * time_buffer); //BufferSize=サンプリング周波数*秒数
    private System.Random random = new System.Random(); // 疑似データ生成用
    public bool trainingmode = false;


    // 1チャネル・1秒（250サンプル）の正例
    private List<double[]> initialPositiveSamples = new List<double[]>
    {
        Enumerable.Repeat(1.0, (int)(fs*time_buffer)).ToArray() // 正例1

    };

    // 1チャネル・1秒（250サンプル）の負例
    private List<double[]> initialNegativeSamples = new List<double[]>
    {
        Enumerable.Repeat(-1.0, (int)(fs*time_buffer)).ToArray()// 負例1
    };

    void Start()
    {
        
        // 修正: インスタンスなしで直接呼び出す
        OnlineLDAClassifier.Initialize(initialPositiveSamples, initialNegativeSamples);
        ModelManager.LoadModel();

    }

    public void StartTraining(bool label)
    {
        StartCoroutine(GenerateFakeEEGData(label));//呼ばれたとき1s間データを保持して学習
    }

    IEnumerator GenerateFakeEEGData(bool label)//疑似EEGデータ生成
    {
        while (true)
        {
            
            float[] fakeEEG = new float[8];
            for (int i = 0; i < 8; i++)
            {
                fakeEEG[i] = (float)(random.NextDouble() * 2.0 - 1.0);
            }

            if (eegBuffer.Count >= BufferSize)//BufferSize=サンプリング周波数*秒数
            {
                trainingBuffer = new Queue<float[]>(eegBuffer);
                ProcessEEGData(label);
                eegBuffer.Clear();
                break;
            }
            eegBuffer.Enqueue(fakeEEG);
            yield return new WaitForSeconds(1f / 250f);//サンプリング周波数ごとにBufferでデータを取得
        }
    }

    private void ProcessEEGData(bool label)
    {


        int timeLength = trainingBuffer.Count;

        float[][] eegDataArray = trainingBuffer.ToArray();
        float[] currentEEG = new float[timeLength];

        for (int t = 0; t < timeLength; t++)
        {
            currentEEG[t] = eegDataArray[t][ElectrodeIndex]; // 各時点の electrodeIndex 番目を取得
        }

        // 修正: 静的メソッドとして呼び出し
        predicted = OnlineLDAClassifier.Classify(currentEEG.Select(x => (double)x).ToArray());
        Debug.Log($"教師データ:{label}分類結果: {predicted}");

        if (trainingmode) 
        { 
            bool trueLabel = label;
            OnlineLDAClassifier.UpdateWithFeedback(currentEEG.Select(x => (double)x).ToArray(), trueLabel);
        }
    }


    
}
