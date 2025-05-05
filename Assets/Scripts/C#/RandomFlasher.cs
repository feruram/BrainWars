
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomFlasher : MonoBehaviour
{
    public GameObject[] objects;  // �T�C�Y4��GameObject�z��
    public int PredictedGameObjectIndex = 0;
    public bool TestLabel = false;
    public float flashDuration = 0.3f;
    public float interval = 0.5f;
    private Color defaultColor;
    private Color flashColor = Color.white;
    public int TimeofStimulus=10;
    int counter_Defaultcolor = 1;
    private List<int> remainingIndices; // �_�ł���Ă��Ȃ��I�u�W�F�N�g��ǐ�
    private System.Random random = new System.Random(); // ���\���s�\�ȃ����_�������m��

    public int currentFlashingIndex = -1; // �I�𒆂̃I�u�W�F�N�g�̔ԍ�
    [SerializeField] EEGReceiver erc;
    void Start()
    {
        SetAllToDefaultColor();
        InitializeRemainingIndices();
        StartCoroutine(FlashRoutine()); // �R���[�`���J�n
    }


    IEnumerator FlashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // interval �b�ҋ@


            FlashNextObject();
            if (counter_Defaultcolor > 10) {
                ModelManager.SaveModel();
                break;

            }
        }
    }

    void FlashNextObject()
    {
        if (remainingIndices.Count == 0)
        {
            // �S�Ă��_�ł�����V�����Z�b�g���J�n
            InitializeRemainingIndices();
            counter_Defaultcolor++;
        }

        // �����_����1��I�����A�_�ł�����
        currentFlashingIndex = remainingIndices[random.Next(remainingIndices.Count)];
        remainingIndices.Remove(currentFlashingIndex); // �I���ς݂̃I�u�W�F�N�g�����X�g����폜
        objects[currentFlashingIndex].GetComponent<Renderer>().material.color = flashColor;

        // �o��: �_�Œ��̃I�u�W�F�N�g�ԍ�
        Debug.Log(currentFlashingIndex);
        
        if (currentFlashingIndex == PredictedGameObjectIndex)
        {
            TestLabel = true;
            erc.StartTraining(TestLabel);
            TestLabel = false;
        }
        else { erc.StartTraining(TestLabel); }


        Invoke("ResetFlash", flashDuration);
    }

    void ResetFlash()
    {
        if (currentFlashingIndex >= 0)
        {
            objects[currentFlashingIndex].GetComponent<Renderer>().material.color = defaultColor;
            currentFlashingIndex = -1;

            // �o��: ��_�ŏ��
            //Debug.Log(-1);

        }
    }

    void SetAllToDefaultColor()
    {
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<Renderer>().material.color = defaultColor;
        }
    }

    void InitializeRemainingIndices()
    {
        remainingIndices = new List<int> { 0, 1, 2}; // �S�I�u�W�F�N�g�����X�g�ɒǉ�
    }
}
