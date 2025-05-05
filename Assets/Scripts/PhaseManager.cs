using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private PlayerPoser player;
    private BCIManager bciManager;
    public float phase=0;

    public float eyeHeight;
    public bool playing=false;
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPoser>();
        bciManager=GameObject.Find("BCIManager").GetComponent<BCIManager>();
    }
    public void NextPhase(){
        if(phase<1){
            phase=1;
        }else if(phase<2){
            phase=2;
        }else if(phase<3){
            phase=3;
        }else if(phase<4){
            phase=4;
        }else if(phase<5){
            phase=5;
        }
    }
    public void SetPhase(int i){
        phase=i;
    }
    public void HalfPhase(){
        phase+=0.5f;
    }
    //0:body calibration
    //1:ERP calibration
    //2:?? calibration
    //3: check
    void Update()
    {
        if(phase==0){
            this.transform.GetChild(2).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }else if(phase==1){
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }else if(phase==4){
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(true);
        }else if(phase==5){
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }

        if(playing){
            bciManager.SetTagsPosition(player.leftHand);
            player.Visualize(true);
            bciManager.ApplicationStart();
        }else{
            player.Visualize(false);
        }
    }

}
