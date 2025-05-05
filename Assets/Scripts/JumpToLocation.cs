using UnityEngine;
public class JumpToLocation : MonoBehaviour
{
    private int warpNumber = 0; // ワープ先のインデックス
    private Transform warpTarget1; // ワープ先のターゲット位置
    private Transform warpTarget2; // ワープ先のターゲット位置
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerOn!!");
        Debug.Log(other.gameObject.name);
        warpTarget1=GameObject.Find("point1").transform;
        warpTarget2=GameObject.Find("point2").transform;
        if (other.CompareTag("WarpTrigger"))
        {
            warpNumber++;
            Debug.Log("warpNumber: " + warpNumber);
            if (warpNumber == 1)
            {
                transform.position = warpTarget1.position;
                transform.rotation = Quaternion.Euler(0, -90, 0); // Y軸に-90度回転
                Debug.Log("ワープ完了（1）");
            }
            else if (warpNumber == 2)
            {
                transform.position = warpTarget2.position;
                transform.rotation = Quaternion.Euler(0, 90, 0); // Y軸に90度回転
                Debug.Log("ワープ完了（2）");
            }
            // ここでPlayerStatusにwarpNumberを渡す
            PlayerStatus status = GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.SetID(warpNumber);
                Debug.Log("PlayerStatusにIDを設定: " + warpNumber);
            }
            else
            {
                Debug.LogWarning("PlayerStatusが見つかりません！");
            }
        }
    }
}