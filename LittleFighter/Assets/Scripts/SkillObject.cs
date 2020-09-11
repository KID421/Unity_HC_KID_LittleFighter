using UnityEngine;

public class SkillObject : MonoBehaviour
{
    /// <summary>
    /// 技能物件傷害值
    /// </summary>
    [HideInInspector]
    public float damage;
    /// <summary>
    /// 技能物件是由哪個玩家發出
    /// </summary>
    [HideInInspector]
    public GameObject player;

    private void Start()
    {
        Destroy(gameObject, 4);                                                                                 // 刪除物件
        ParticleSystem.CollisionModule collision = GetComponent<ParticleSystem>().collision;                    // 取得粒子碰撞
        if (player.layer == 8) collision.collidesWith = 1 << 9;                                                 // 判斷玩家如果 為 8 圖層 設定跟 9 圖層碰撞
        else collision.collidesWith = 1 << 8;                                                                   // 否則 跟 8 圖層碰撞
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<FighterControl>() && collision.gameObject.gameObject != player)   // 如果碰到的物件有對戰控制器 並且 非發出玩家
        {
            Destroy(gameObject, 0.3f);                                                                          // 碰撞後延遲刪除
            GetComponent<Rigidbody>().velocity = Vector3.zero;                                                  // 加速度歸零
            GetComponent<SphereCollider>().enabled = false;                                                     // 關閉碰撞
            collision.gameObject.GetComponent<FighterControl>().Damage(damage);                                 // 對其造成傷害
        }
        else Destroy(gameObject);                                                                               // 碰到其他東西直接刪除
    }
}
