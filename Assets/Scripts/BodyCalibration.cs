using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCalibration : MonoBehaviour
{
    public float defaultAvatarHeight = 1.7f;

    public Transform playerCamera;
    public float animationTime=3.0f;
    private Transform playerAvatar;
    private ControllerInputManager input;
    private PhaseManager phaseManager;
    // 目線から頭頂までの補正係数（93%が目線という前提）
    private const float eyeToTopRatio = 0.93f;
    void Start(){
        playerAvatar=this.transform.parent.transform.GetChild(0);
        input=GameObject.FindGameObjectWithTag("Input").GetComponent<ControllerInputManager>();
        phaseManager=this.transform.parent.GetComponent<PhaseManager>();
    }
    void Update()
    {
        if(phaseManager.phase==0){
            this.transform.GetChild(1).position=new Vector3(0,playerCamera.position.y,0.6f);
            if(input.trigger){
                if (playerCamera == null)
                {
                    playerCamera = Camera.main?.transform;
                }
                if (playerCamera == null || playerAvatar == null)
                {
                    Debug.LogError("Camera または Avatar が設定されていません");
                    return;
                }
                float eyeHeight = playerCamera.position.y;
                phaseManager.eyeHeight=eyeHeight;
                float estimatedHeight = eyeHeight / eyeToTopRatio;
                float scaleFactor = estimatedHeight / defaultAvatarHeight;
                playerAvatar.localScale = Vector3.one * scaleFactor;
                Debug.Log($"推定身長: {estimatedHeight:F2}m, スケール倍率: {scaleFactor:F2}");
                phaseManager.HalfPhase();
                StartCoroutine(Next());
            }
        }
    }
    private IEnumerator Next(){
        float elapsedTime=0;
        while(elapsedTime<animationTime){
            elapsedTime+=Time.deltaTime;
            this.transform.position=new Vector3(0,-5*(elapsedTime/animationTime),0);
            yield return null;
        }
        phaseManager.NextPhase();
    }
}
