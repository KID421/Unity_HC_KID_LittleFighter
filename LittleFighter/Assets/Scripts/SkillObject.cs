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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FighterControl>() && other.gameObject != player)         // 如果碰到的物件有對戰控制器 並且 非發出玩家
        {
            other.GetComponent<FighterControl>().Damage(damage);                        // 對其造成傷害
        }
    }
}
