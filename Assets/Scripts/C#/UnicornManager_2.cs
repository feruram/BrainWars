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
    public InputField deviceInputField;  // UI���̓t�B�[���h
    public int deviceId = 0;
    private bool time_getdata_bool =true;
    private float time_getdata = 0.5f;
    private Unicorn device;             // �f�o�C�X
    private uint numberOfAcquiredChannels;
    private uint FrameLength = 1;
    byte[] receiveBuffer;
    GCHandle receiveBufferHandle;
    [SerializeField]bool Buttononclicked = false;
    [SerializeField]public float[] eegValues;
    //eegValues�͂O�|�P�V�̔z��
    /// <summary>
    /// EEG 1|2|3|4|5|6|7|8
    /// �����x�v X|Y|Z
    /// �W���C���X�R�[�v X|Y|Z
    /// �J�E���^�[
    /// �o�b�e���[���x��
    /// ���؃C���W�P�[�^�[
    /// </summary>
    void Start()
    {
        DisplayAvailableDevices();
    }
    private void Update()
    {
        // �L�[����������f�[�^�擾���~
        if (Input.GetKeyDown(KeyCode.Space))  // ��: �X�y�[�X�L�[�������ƒ�~
        {
            StopAcquisition();
        }
        //�������f�o�C�X���ݒ�ł��Ă���A�{�^�����N���b�N����Ă���Ύ��s����
        if (device != null && Buttononclicked&&time_getdata_bool)
        {
            //FrameLength �� �擾����f�[�^�̃t���[����
            //receiveBufferHandle.AddrOfPinnedObject() �� �擾�����f�[�^��ۑ�����o�b�t�@�̃|�C���^
            //(uint)(receiveBuffer.Length / sizeof(float)) �� �擾����f�[�^�̃T���v����

            device.GetData(FrameLength, receiveBufferHandle.AddrOfPinnedObject(), (uint)(receiveBuffer.Length / sizeof(float)));
            
            //�o�C�i���f�[�^����Float�^�֕ϊ�
            Buffer.BlockCopy(receiveBuffer, 0, eegValues, 0, receiveBuffer.Length);
            //Debug.Log($"Data: {string.Join(", ", receiveBuffer)}"); // �f�[�^���o��
            float eegValue = eegValues[0] / 100000;  // ��: �`�����l��0�̒l
                                                     // �V�����_�����X�g�ɒǉ�
                                                     // �V�����f�[�^�|�C���g�����X�g�ɒǉ�

        }



    }

    IEnumerator AcquireData()
    {
        while (Buttononclicked)
        {
            yield return new WaitForSeconds(30.0f); // 30s�f�[�^�擾
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
        device.StartAcquisition(false); // �f�[�^�擾�J�n
    }
    void StopAcquisition()
    {
        if (device != null && Buttononclicked)
        {
            Buttononclicked = false; // �t���O���I�t�ɂ��čX�V���~
            device.StopAcquisition();
            Debug.Log("Data acquisition stopped.");
            
            receiveBufferHandle.Free();  // �����ŉ������
            device.Dispose();

        }
        else
        {
            Debug.Log("�f�o�C�X���ݒ肳��Ă��܂���");
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

    void DisplayAvailableDevices()//���łɃy�A�����O���Ă���A�ڑ��\�ȃf�o�C�X���R���\�[���ɕ\��
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


    //�g�p�ł���f�o�C�X����A�f�[�^���擾����f�o�C�X��InputField�ɓ��͂��Ďg�p�\�ɂ���
    //Button����������A�Ή�����f�o�C�X����Update�֐��̃��[�v���device.GetData�Ńf�[�^�擾
    public void StartAcquisition()
    {
        
        try
        {
            IList<string> devices = Unicorn.GetAvailableDevices(true);
            deviceId = int.Parse(deviceInputField.text); // ���[�U�[���͂��擾

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
            device.StartAcquisition(false); // �f�[�^�擾�J�n
            Debug.Log("Data acquisition started.");
            Buttononclicked = true;
            StartCoroutine(AcquireData());
            // Allocate memory for the acquisition buffer.
            // receiveBuffer�F�f�[�^�̎�M�o�b�t�@�̒����ƈ���A�h���X�̐ݒ�
            //�@GCHandleType.Pinned���A�h���X
            //�@GCHandle.Alloc(receiveBuffer, GCHandleType.Pinned); �Ń������m��
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