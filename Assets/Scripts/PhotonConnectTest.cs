using UnityEngine;
using Photon.Pun;
public class PhotonConnectTest : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photonに接続成功！");
    }
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("Photon切断: " + cause.ToString());
    }
}