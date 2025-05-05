using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions.Must;
using TMPro;
public class ERPCalibration : MonoBehaviour
{
    private PlayerPoser player;
    private ControllerInputManager input;
    private PhaseManager phaseManager;
    private GameObject canvas;
    private GameObject tags;
    private GameObject checkUI;
    private BCIManager bciManager;
    private TextMeshProUGUI statsText;
    
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPoser>();
        input=GameObject.FindGameObjectWithTag("Input").GetComponent<ControllerInputManager>();
        phaseManager=this.transform.parent.GetComponent<PhaseManager>();
        canvas=this.transform.GetChild(0).gameObject;
        tags=this.transform.GetChild(1).gameObject;
        checkUI=this.transform.GetChild(2).gameObject;
        bciManager=GameObject.Find("BCIManager").GetComponent<BCIManager>();
        checkUI.SetActive(false);
        statsText=checkUI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(phaseManager.phase==1){
            canvas.transform.position=new Vector3(0,phaseManager.eyeHeight-0.2f,0.9f);
            tags.transform.position=new Vector3(0,phaseManager.eyeHeight-0.4f,0.6f);
            checkUI.transform.position=new Vector3(0,phaseManager.eyeHeight,1.6f);
            bciManager.SetTagsPosition(tags.transform);
            canvas.SetActive(true);
            checkUI.SetActive(false);
            if(input.trigger){
                canvas.SetActive(false);
                phaseManager.HalfPhase();
                bciManager.ConnectBtn();
                StartCoroutine(waitConnection());
            }
        }else if(phaseManager.phase==2){
            canvas.SetActive(false);
            checkUI.SetActive(false);
            bciManager.TrainingBtn();
            phaseManager.HalfPhase();
            StartCoroutine(waitTraining());
        }else if(phaseManager.phase==3){
            statsText.text=bciManager.trainingResult;
            checkUI.SetActive(true);
            if(input.A){
                phaseManager.SetPhase(2);
            }else if(input.trigger){
                phaseManager.NextPhase();
            }
        }
    }
    IEnumerator waitConnection(){
        while(true){
            if(bciManager.isConnected){
                break;
            }
            yield return null;
        }
        phaseManager.NextPhase();
    }
    IEnumerator waitTraining(){
        while(true){
            if(bciManager.isTrainingStopped){
                break;
            }
            yield return null;
        }
        phaseManager.NextPhase();
    }
}
