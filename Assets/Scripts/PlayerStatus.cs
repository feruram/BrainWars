using UnityEngine;
public class PlayerStatus : MonoBehaviour
{
    // プレイヤー固有のID（warpNumberと同義）
    public int myID { get; private set; }
    // HPの設定（任意で初期値変更可）
    public int maxHP = 10;
    public int currentHP;
    private BattleManager battle;
    private void Start()
    {
        currentHP = maxHP;
        battle=GameObject.FindGameObjectWithTag("Battle").GetComponent<BattleManager>();
    }
    // IDを外部から設定する関数（スポーン時に呼ぶ）
    public void SetID(int id)
    {
        myID = id;
    }
    // 攻撃時に使う：自分と異なるIDを持つ相手かどうか
    public bool IsEnemy(int attackerID)
    {
        return myID != attackerID;
    }
    public void Damage(int damage){
        currentHP-=damage;
        if(currentHP<0){
            if(myID==1){
                battle.Finish(2);
            }else{
                battle.Finish(1);
            }
        }
    }
}