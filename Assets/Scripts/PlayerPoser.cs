using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoser : MonoBehaviour
{
    public Transform rightHand;
    public Transform leftHand;
    public Transform thumb;
    public Transform index;
    public Transform middle;
    public Transform ring;
    public Transform little;
    public GameObject face;
    public GameObject body;
    public GameObject weapon;
    public void Visualize(bool flag){
        face.SetActive(flag);
        body.SetActive(flag);
        weapon.SetActive(flag);
    }
    public void ChangePose(int c){
        if(c==1){
            thumb.transform.localEulerAngles=new Vector3(30,0,-30);
            thumb.transform.GetChild(0).localEulerAngles=new Vector3(50,0,-30);
            thumb.transform.GetChild(0).GetChild(0).localEulerAngles=new Vector3(0,40,0);
            index.transform.localEulerAngles=new Vector3(0,0,-40);
            index.transform.GetChild(0).localEulerAngles=new Vector3(0,0,-20);
            index.transform.GetChild(0).GetChild(0).localEulerAngles=new Vector3(0,0,-30);
            middle.transform.localEulerAngles=new Vector3(0,0,-50);
            middle.transform.GetChild(0).localEulerAngles=new Vector3(0,0,-40);
            middle.transform.GetChild(0).GetChild(0).localEulerAngles=new Vector3(0,0,-30);
            ring.transform.localEulerAngles=new Vector3(0,0,-60);
            ring.transform.GetChild(0).localEulerAngles=new Vector3(0,0,-40);
            ring.transform.GetChild(0).GetChild(0).localEulerAngles=new Vector3(0,0,-30);
            little.transform.localEulerAngles=new Vector3(0,0,-70);
            little.transform.GetChild(0).localEulerAngles=new Vector3(0,0,-40);
            little.transform.GetChild(0).GetChild(0).localEulerAngles=new Vector3(0,0,-30);
        }
    }
}
