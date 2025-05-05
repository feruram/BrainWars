using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float spellerTime=15;
    private float setTimerStart=0;
    public void SpellerInit(){
        setTimerStart=Time.time;
    }
    public bool CanFakeSpeller(){
        if(Time.time-setTimerStart>spellerTime){
            return true;
        }else{
            return false;
        }
    }
}
