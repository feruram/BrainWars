using UnityEngine;

public class ShieldSkillController : MonoBehaviour
{
    public GameObject shockwaveEffectPrefab; // �Ռ��g�̃G�t�F�N�g
    public int myId=0;
    public float shockwaveRadius = 5f;
    public float shockwaveDamage = 20f;
    public LayerMask enemyLayer;
    public int miDamage=2;

    // �����֐�
    public void ActivateShockwave()
    {
        if (shockwaveEffectPrefab != null)
        {
            GameObject effect = Instantiate(shockwaveEffectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<Damager>().damage=miDamage;
            effect.GetComponent<Damager>().myId=myId;
            Destroy(effect, 5f);
        }

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, shockwaveRadius, enemyLayer);
        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
            // enemy.GetComponent<EnemyHealth>()?.TakeDamage(shockwaveDamage);
        }
    }
}
