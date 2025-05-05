using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ERPTest : MonoBehaviour
{
    private PlayerPoser player;
    private ControllerInputManager input;
    private PhaseManager phaseManager;
    private BCIManager bciManager;
    private SkillManager skillManager;
    private GameObject check1;
    private GameObject check2;
    private GameObject check3;
    [SerializeField]private bool one=false;
    [SerializeField]private bool two=false;
    [SerializeField]private bool three=false;
    [SerializeField]private bool mi=false;
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPoser>();
        input=GameObject.FindGameObjectWithTag("Input").GetComponent<ControllerInputManager>();
        phaseManager=this.transform.parent.GetComponent<PhaseManager>();
        skillManager=this.transform.parent.GetComponent<SkillManager>();
        bciManager=GameObject.Find("BCIManager").GetComponent<BCIManager>();
        check1=this.transform.GetChild(0).GetChild(0).gameObject;
        check2=this.transform.GetChild(0).GetChild(1).gameObject;
        check3=this.transform.GetChild(0).GetChild(2).gameObject;
        check1.SetActive(false);
        check2.SetActive(false);
        check3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(phaseManager.phase==4){
            phaseManager.playing=true;
            switch(skillManager.currentStimulus){
                case "ONE":
                    one=true;
                break;
                case "TWO":
                    two=true;
                break;
                case "THREE":
                    three=true;
                break;
            }
            check1.SetActive(one);
            check2.SetActive(two);
            check3.SetActive(three);
            if(one&&two&&three){
                phaseManager.NextPhase();
            }
        }   
    }
}
