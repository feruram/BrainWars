using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Valve.VR.InteractionSystem;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string avatarName = "Avatar";
    [SerializeField] Transform spawnpoint;
    // Photonマスターサーバーに接続
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    // 接続成功後、Roomという名前のルームに入る（存在しなければ作る）
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }
    // ルーム入室時にプレイヤー生成
    public override void OnJoinedRoom()
    {
        // スポーン地点の座標を取得
        Vector3 position = spawnpoint.position;
        // Avatarをスポーン
        GameObject player = PhotonNetwork.Instantiate(avatarName, position, Quaternion.identity);
        Debug.Log("Spawned player position: " + player.transform.position);
    }
}