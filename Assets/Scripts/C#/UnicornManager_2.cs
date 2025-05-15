using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Gtec.Unicorn;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class UnicornManager_2 : MonoBehaviour
{
    public InputField deviceInputField;  // UI入力フィールド
    public int deviceId = 0;
    private bool time_getdata_bool =true;
    private float time_getdata = 0.5f;
    private Unicorn device;             // デバイス
    private uint numberOfAcquiredChannels;
    private uint FrameLength = 1;
    byte[] receiveBuffer;
    GCHandle receiveBufferHandle;
    [SerializeField]bool Buttononclicked = false;
    [SerializeField]public float[] eegValues;
    //eegValuesは０－１７の配列
    /// <summary>
    /// EEG 1|2|3|4|5|6|7|8
    /// 加速度計 X|Y|Z
    /// ジャイロスコープ X|Y|Z
    /// カウンター
    /// バッテリーレベル
    /// 検証インジケーター
    /// </summary>
    void Start()
    {
        DisplayAvailableDevices();
    }
    private void Update()
    {
        // キーを押したらデータ取得を停止
        if (Input.GetKeyDown(KeyCode.Space))  // 例: スペースキーを押すと停止
        {
            StopAcquisition();
        }
        //正しくデバイスが設定できており、ボタンがクリックされていれば実行する
        if (device != null && Buttononclicked&&time_getdata_bool)
        {
            //FrameLength → 取得するデータのフレーム数
            //receiveBufferHandle.AddrOfPinnedObject() → 取得したデータを保存するバッファのポインタ
            //(uint)(receiveBuffer.Length / sizeof(float)) → 取得するデータのサンプル数

            device.GetData(FrameLength, receiveBufferHandle.AddrOfPinnedObject(), (uint)(receiveBuffer.Length / sizeof(float)));
            
            //バイナリデータからFloat型へ変換
            Buffer.BlockCopy(receiveBuffer, 0, eegValues, 0, receiveBuffer.Length);
            //Debug.Log($"Data: {string.Join(", ", receiveBuffer)}"); // データを出力
            float eegValue = eegValues[0] / 100000;  // 例: チャンネル0の値
                                                     // 新しい点をリストに追加
                                                     // 新しいデータポイントをリストに追加

        }



    }

    IEnumerator AcquireData()
    {
        while (Buttononclicked)
        {
            yield return new WaitForSeconds(30.0f); // 30sデータ取得
            time_getdata_bool = false;
            ResetBuffer();
            Array.Fill(eegValues, 0f);
            yield return new WaitForSeconds(time_getdata); // time_getdata s
            time_getdata_bool = true;
            
        }
    }

    void ResetBuffer()
    {
        device.Dispose();
        IList<string> devices = Unicorn.GetAvailableDevices(true);
        device = new Unicorn(devices.ElementAt(deviceId));
        device.StartAcquisition(false); // データ取得開始
    }
    void StopAcquisition()
    {
        if (device != null && Buttononclicked)
        {
            Buttononclicked = false; // フラグをオフにして更新を停止
            device.StopAcquisition();
            Debug.Log("Data acquisition stopped.");
            
            receiveBufferHandle.Free();  // ここで解放する
            device.Dispose();

        }
        else
        {
            Debug.Log("デバイスが設定されていません");
            device.Dispose();
        }
    }
    void OnApplicationQuit()
    {
        try
        {
            device.StopAcquisition();
            device.Dispose();
            Debug.Log("Data acquisition stopped.");
        }
        catch (DeviceException ex)
        {
            Debug.LogError($"StopAcquisition failed: {ex.Message}");
        }
    }

    void DisplayAvailableDevices()//すでにペアリングしてあり、接続可能なデバイスをコンソールに表示
    {
        IList<string> devices = Unicorn.GetAvailableDevices(true);
        if (devices.Count < 1)
        {
            Debug.LogError("No device available. Please pair with a Unicorn first.");
            return;
        }

        Debug.Log("Available devices:");
        for (int i = 0; i < devices.Count; i++)
        {
            Debug.Log("#" + i + ": " + devices.ElementAt(i));
        }
    }


    //使用できるデバイスから、データを取得するデバイスをInputFieldに入力して使用可能にする
    //Buttonを押したら、対応するデバイスからUpdate関数のループよりdevice.GetDataでデータ取得
    public void StartAcquisition()
    {
        
        try
        {
            IList<string> devices = Unicorn.GetAvailableDevices(true);
            deviceId = int.Parse(deviceInputField.text); // ユーザー入力を取得

            if (deviceId >= devices.Count || deviceId < 0)
            {
                Debug.LogError("Invalid device ID.");
                return;
            }

            device = new Unicorn(devices.ElementAt(deviceId));
            Debug.Log($"Connected to {devices.ElementAt(deviceId)}");
            numberOfAcquiredChannels = device.GetNumberOfAcquiredChannels();
            Debug.Log(numberOfAcquiredChannels.ToString());
            Debug.Log(Unicorn.SamplingRate);
            Debug.Log(Unicorn.EEGConfigIndex);
            device.StartAcquisition(false); // データ取得開始
            Debug.Log("Data acquisition started.");
            Buttononclicked = true;
            StartCoroutine(AcquireData());
            // Allocate memory for the acquisition buffer.
            // receiveBuffer：データの受信バッファの長さと宛先アドレスの設定
            //　GCHandleType.Pinnedがアドレス
            //　GCHandle.Alloc(receiveBuffer, GCHandleType.Pinned); でメモリ確保
            receiveBuffer = new byte[FrameLength * sizeof(float) * numberOfAcquiredChannels];
            receiveBufferHandle = GCHandle.Alloc(receiveBuffer, GCHandleType.Pinned);
            eegValues = new float[receiveBuffer.Length / sizeof(float)];

        }
        catch (Exception ex)
        {
            Debug.LogError($"StartAcquisition failed: {ex.Message}");
        }
        if (device == null)
        {
            Debug.LogError("Device not initialized.");
            return;
        }

    }
}