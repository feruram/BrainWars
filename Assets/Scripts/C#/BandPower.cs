using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using System;
using Gtec.Unicorn;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BandPower : MonoBehaviour
{
    /// <summary>
    /// �ш悲�Ƃ�BandPower�����߂�B
    /// ���ߕ��͎��g���ш敪�̃p���[�����Z
    /// int deviceId = int.Parse(deviceInputField.text); // ユーザー入力を取得
    /// </summary>
    [SerializeField] UnicornManager_2 unicornManager;
    const int TimeWidth = 2;//���b�Ɉ��Z�o���邩
    private const int SampleRate = 250;  // 250Hz
    private const int BufferSize = SampleRate * TimeWidth; // 2�b�� = 500�T���v��
    public Queue<float[]> eegBuffer = new Queue<float[]>();
    // チャンネルごとのバンドパワー格納用
    public float[] deltaPower, thetaPower, alphaPower, betaPower;
    //public float[] deltaPower = new float[8];
    //public float[] thetaPower = new float[8];
    //public float[] alphaPower = new float[8];
    //public float[] betaPower = new float[8];


    //float[8]��8�`�����l�����̂PSample�f�[�^��500��Queue�Ŏ擾�������
    // Start is called before the first frame update
    void Start()
    {
        deltaPower = new float[8];
        thetaPower = new float[8];
        alphaPower = new float[8];
        betaPower = new float[8];
        StartCoroutine(EEGDataCollection());
        StartCoroutine(ProcessEEGData());

    }
    IEnumerator EEGDataCollection()
    {
        while (true)
        {
            // �f�[�^�̒������m�F
            if (unicornManager.eegValues.Length < 8)
            {
                Debug.LogWarning($"�f�[�^�擾���s: unicornManager.eegValues.Length = {unicornManager.eegValues.Length}");
                yield return new WaitForSeconds(1f / SampleRate); // ���̃f�[�^�擾�܂ő҂�
                continue; // �f�[�^�擾���s���̓X�L�b�v
            }
            else
            {
                // �f�[�^�擾�ƒǉ�
                float[] eegData = unicornManager.eegValues.Take(8).ToArray();

                if (eegBuffer.Count >= BufferSize)//500Sample����Queue�𒴂��Ă���Ώ�����
                {
                    eegBuffer.Dequeue(); // �Â��f�[�^���폜
                }

                eegBuffer.Enqueue(eegData); // �L���ȃf�[�^�̂ݒǉ�

                yield return new WaitForSeconds(1f / SampleRate); // 250Hz�Ŏ��W
            }
        }
    }
    IEnumerator ProcessEEGData()
    {
        while (true)
        {
            if (eegBuffer.Count >= BufferSize)
            {
                // 500�T���v�����̃f�[�^���擾�i500�~8�j
                float[][] eegDataArray = eegBuffer.ToArray();

                // �e�`�����l���̎��n��f�[�^���쐬
                float[][] channelData = new float[8][];
                for (int ch = 0; ch < 8; ch++)
                {
                    channelData[ch] = new float[BufferSize];
                    Debug.Log(channelData[ch].Length);
                }



                for (int i = 0; i < BufferSize; i++)
                {
                    for (int ch = 0; ch < 8; ch++)
                    {
                        Debug.Log(eegDataArray[i].Length);
                        channelData[ch][i] = eegDataArray[i][ch]; // �e�`�����l�����Ƃ̎��n��f�[�^
                    }
                }

                // 各チャンネルに対して FFT & バンドパワー計算
                for (int ch = 0; ch < 8; ch++)
                {
                    Complex[] complexData = channelData[ch].Select(x => new Complex(x, 0)).ToArray();
                    Complex[] spectrum = FFT(complexData);
                    Debug.Log($"spectrum:{spectrum.Length}");

                    deltaPower[ch] = ComputeBandPower(spectrum, 1, 4, SampleRate, BufferSize);
                    thetaPower[ch] = ComputeBandPower(spectrum, 4, 8, SampleRate, BufferSize);
                    alphaPower[ch] = ComputeBandPower(spectrum, 8, 13, SampleRate, BufferSize);
                    betaPower[ch] = ComputeBandPower(spectrum, 13, 30, SampleRate, BufferSize);

                    Debug.Log($"Ch{ch} - Delta: {deltaPower[ch]}dB, Theta: {thetaPower[ch]}dB, Alpha: {alphaPower[ch]}dB, Beta: {betaPower[ch]}dB");
                }

            }

            yield return new WaitForSeconds(TimeWidth); // 2�b���Ƃ�FFT���s
        }
    }

    public Complex[] FFT(Complex[] data)
    {
        int N = data.Length;
        if (N <= 1) return data;

        Complex[] even = new Complex[N / 2];
        Complex[] odd = new Complex[N / 2];

        for (int i = 0; i < N / 2; i++)
        {
            even[i] = data[i * 2];
            odd[i] = data[i * 2 + 1];
        }

        Complex[] fftEven = FFT(even);
        Complex[] fftOdd = FFT(odd);

        Complex[] result = new Complex[N];
        for (int k = 0; k < N / 2; k++)
        {
            Complex t = Complex.FromPolarCoordinates(1, -2 * Math.PI * k / N) * fftOdd[k];
            result[k] = fftEven[k] + t;
            result[k + N / 2] = fftEven[k] - t;
        }

        return result;
    }
    float ComputeBandPower(Complex[] spectrum, int startFreq, int endFreq, float sampleRate, int fftSize)
    {
        int startIdx = (int)(startFreq * fftSize / sampleRate);
        int endIdx = (int)(endFreq * fftSize / sampleRate);

        // IndexOutOfRange防止
        endIdx = Mathf.Min(endIdx, spectrum.Length - 1);

        float powerSum = 0f;
        for (int i = startIdx; i <= endIdx; i++)
        {
            powerSum += (float)(spectrum[i].Magnitude * spectrum[i].Magnitude);
        }

        float avgPower = powerSum / (endIdx - startIdx + 1);
        float powerDb = 10f * Mathf.Log10(avgPower + 1e-10f); // ゼロ除算防止

        return powerDb;
    }

}
