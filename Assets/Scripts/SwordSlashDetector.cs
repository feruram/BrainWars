using UnityEngine;

public class SwordSlashDetector : MonoBehaviour
{
    public float swingThreshold = 2.0f; // 斬撃と判定する速度の閾値
    public GameObject slashEffectPrefab; // 斬撃エフェクトのプレハブ
    public Transform playerCamera; // プレイヤーのカメラ（正面方向を取得）
    public float cooldownTime = 0.5f; // クールダウン時間（秒）
    public int myId=0;
    private Vector3 previousPosition;
    private float lastSlashTime = -Mathf.Infinity;
    public bool charged=false;
    public int normalDamage=1;
    public int miDamage=2;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - previousPosition;
        float speed = movement.magnitude / Time.deltaTime;
        Vector3 swingDirection = movement.normalized;
        previousPosition = currentPosition;
        this.gameObject.GetComponent<Damager>().damage=normalDamage;
        this.gameObject.GetComponent<Damager>().myId=myId;
        float bladeAlignment = Vector3.Dot(transform.right, swingDirection);
        if (speed > swingThreshold && bladeAlignment > 0.5f&&charged)
        {
            if (Time.time >= lastSlashTime + cooldownTime)
            {
                TriggerSlashEffect(swingDirection);
                lastSlashTime = Time.time;
                charged=false;
            }
        }
    }

    void TriggerSlashEffect(Vector3 swingDirection)
    {
        Vector3 spawnPosition = transform.position;
        Vector3 forwardDirection = playerCamera.forward;

        Quaternion effectRotation = Quaternion.LookRotation(forwardDirection, swingDirection);

        GameObject slashEffect = Instantiate(slashEffectPrefab, spawnPosition, effectRotation);
        slashEffect.GetComponent<Damager>().myId=myId;
        slashEffect.GetComponent<Damager>().damage=miDamage;
        Destroy(slashEffect, 2.0f);
    }
    public void Charge()
    {
        charged=true;
    }
}
