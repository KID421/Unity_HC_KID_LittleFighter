using UnityEngine;

[CreateAssetMenu(fileName = "角色資料", menuName = "KID/角色資料")]
public class CharacterData : ScriptableObject
{
    [Header("圖像")]
    public Sprite sprite;
    [Header("名稱")]
    public string _name;
    [Header("移動速度"), Range(0.1f, 50f)]
    public float speed;
    [Header("血量"), Range(1, 5000)]
    public float hp = 100;
    [Header("魔力"), Range(1, 2500)]
    public float mp = 50;
    [Header("跳躍高度"), Range(10, 3000)]
    public float jump = 100;
    [Header("攻擊力"), Range(1, 100)]
    public float attack = 10;
    [Header("每秒恢復血量"), Range(0.1f, 10)]
    public float restoreHp = 1;
    [Header("每秒恢復魔力"), Range(0.1f, 10)]
    public float restoreMp = 0.5f;
    [Header("招式")]
    public Skill skill;
}

/// <summary>
/// 招式輸入按鈕列舉
/// </summary>
public enum SkillInput
{
    上, 下, 前, 攻, 防, 跳
}

/// <summary>
/// 技能類型
/// </summary>
public enum SkillType
{
    遠距離, 近距離
}


[System.Serializable]
public class Skill
{
    [Header("名稱")]
    public string name;
    //[Header("技能類型")]
    //public SkillType skillType;
    [Header("按鈕")]
    public SkillInput[] skillInputs = new SkillInput[3];
    [Header("傷害值"), Range(1f, 500f)]
    public float damage;
    [Header("消耗魔力"), Range(10f, 500f)]
    public float cost;
    [Header("延遲時間"), Range(0.1f, 1f)]
    public float interval;
    [Header("生成物")]
    public GameObject skillObject;
    [Header("生成物速度"), Range(100, 3000)]
    public float speed;
}
