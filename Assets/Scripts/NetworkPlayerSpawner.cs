using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    // Avatarという名前のプレハブをネットワーク上に生成
    public string playerPrefabName = "Avatar";
    // すべてのスポーン地点につける共通タグ
    public string spawnPointTag = "SpawnPoint";
    // プレイヤーが Photonのルームに入室した直後に自動で呼ばれる関数
    public override void OnJoinedRoom()
    {
        // SpawnPointタグがついたすべてのスポーン地点を取得
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnPointTag);
        // 名前順にソート（例：SpawnPoint1, SpawnPoint2 になるように）
        System.Array.Sort(spawnPoints, (a, b) => a.name.CompareTo(b.name));
        // 自分のActorNumberに応じたインデックスを計算（0ベースの配列のため -1）
        int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        // プレイヤー数 > スポーン地点数の場合
        if (index >= spawnPoints.Length)
        {
            Debug.LogWarning("SpawnPointが足りません。最後のスポーン地点を使用します。");
            index = spawnPoints.Length - 1;
        }
        // 対応するスポーン地点の位置と回転を取得
        Vector3 spawnPos = spawnPoints[index].transform.position;
        Quaternion spawnRot = spawnPoints[index].transform.rotation;
        // ネットワーク上のアバターを生成
        GameObject avatar = PhotonNetwork.Instantiate(playerPrefabName, spawnPos, spawnRot);
        Debug.Log("Spawned Avatar at: " + avatar.transform.position);
    }
}