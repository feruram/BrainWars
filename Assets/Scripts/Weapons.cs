using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public GameObject sword;
    public GameObject shield;
    public GameObject thunder;
    private int preWeapon=1;
    [SerializeField]private int nowWeapon;
    void Start()
    {
        sword.SetActive(true);
        shield.SetActive(false);
        thunder.SetActive(false);
    }
    public void Equip(int i){
        nowWeapon=i;
        if(i==1&&i!=preWeapon){
            Debug.Log("Equip sword");
            sword.SetActive(true);
            shield.SetActive(false);
            thunder.SetActive(false);
            preWeapon=1;
        }else if(i==2&&i!=preWeapon){
            Debug.Log("Equip shield");
            sword.SetActive(false);
            shield.SetActive(true);
            thunder.SetActive(false);
            preWeapon=2;
        }else if(i==3&&i!=preWeapon){
            Debug.Log("Equip thunder");
            sword.SetActive(false);
            shield.SetActive(false);
            thunder.SetActive(true);
            preWeapon=3;
        }
    }
    public void Burst(){
        if(nowWeapon==1){
            sword.GetComponent<SwordSlashDetector>().Charge();
        }else if(nowWeapon==2){
            shield.GetComponent<ShieldSkillController>().ActivateShockwave();
        }else if(nowWeapon==3){

        }
    }
    
}
