using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damage=1;
    public int myId;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="VRBody"){
            if(other.gameObject.GetComponent<PlayerStatus>().myID!=myId){
            other.gameObject.GetComponent<PlayerStatus>().Damage(damage);
        }
        }
        
    }
}
