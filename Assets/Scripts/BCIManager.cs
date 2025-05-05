using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BCIManager : MonoBehaviour
{
    public GameObject erpTags;
    public bool isConnected=false;
    public bool isTrainingStopped=false;
    public string trainingResult="";
    [SerializeField]private Transform tagsParent;
    public string stateConnect="disConnect";
    void Start()
    {

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){

            ConnectBtn();
        }
        if(stateConnect=="connect"){
            if(Input.GetKeyDown(KeyCode.T)){
            TrainingBtn();
            }
            if(Input.GetKeyDown(KeyCode.S)){
                ApplicationStart();
            }
        }
        erpTags.transform.position=tagsParent.position;
        erpTags.transform.rotation=tagsParent.rotation;
    }
    public void SetTagsPosition(Transform parent){
        tagsParent=parent;
    }
    public void ConnectBtn()
    {

        StartCoroutine(waitConnection());
        if(stateConnect=="connect"){
            stateConnect="disConnect";
            isConnected=false;
        }else{
            stateConnect="connect";
        }

    }
    public void TrainingBtn(){
        isTrainingStopped=false;
    }

    public void ApplicationStart(){
    }

    public void ParadigmStopped(){
        isTrainingStopped=true;
    }
    IEnumerator waitConnection(){
        yield return new WaitForSeconds(10.0f);
        isConnected=true;
    }
}
