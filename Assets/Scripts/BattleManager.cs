using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BattleManager : MonoBehaviour
{
    public int winner=0;
    public TextMeshProUGUI winnerText;
    public GameObject textUI;
    void Start()
    {
        textUI.SetActive(false);
    }
    public void Finish(int i){
        winner=i;
        textUI.SetActive(true);
        winnerText.text="PLAYER "+winner;
    }
}
