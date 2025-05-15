using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private Queue<float[]> receivedEEGBuffer;

    public void GetBufferedData(Queue<float[]> data)
    {
        receivedEEGBuffer = data;
    }

    public float[,] GetBufferedData()
    {
        if (receivedEEGBuffer == null || receivedEEGBuffer.Count == 0)
        {
            Debug.LogWarning("EEG buffer is empty.");
            return new float[0, 0];
        }

        int rows = receivedEEGBuffer.Count;
        int cols = receivedEEGBuffer.Peek().Length;

        float[,] result = new float[rows, cols];
        int i = 0;
        foreach (var row in receivedEEGBuffer)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = row[j];
            }
            i++;
        }

        return result;
    }
}
