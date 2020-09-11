using System.Xml;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 對戰控制系統：移動、攻防跳與動畫
/// </summary>
public class FighterControl : MonoBehaviour
{
    #region 欄位：公開區塊
    [Header("玩家控制資料")]
    public ControlData data;
    [Header("角色資料")]
    public CharacterData dataCharacter;
    [Header("跳躍設定")]
    public Vector3 rayOffset;
    public float length = 0.1f;
    [Header("招式間隔時間")]
    public float interval = 0.3f;
    [Header("技能生成位置")]
    public Transform pointSpawn;
    #endregion

    #region 元件與物件：私人
    private Rigidbody rig;
    private Animator ani;

    /// <summary>
    /// 陰影
    /// </summary>
    private Transform shadow;
    /// <summary>
    /// 畫布
    /// </summary>
    private Transform canvas;
    /// <summary>
    /// 文字：角色名稱
    /// </summary>
    private Text textName;
    /// <summary>
    /// 圖片：血量
    /// </summary>
    private Image imgHp;
    /// <summary>
    /// 圖片：魔力
    /// </summary>
    private Image imgMp;
    #endregion

    #region 欄位：私人
    /// <summary>
    /// 是否在地板上
    /// </summary>
    private bool isGround;
    /// <summary>
    /// 是否防禦中
    /// </summary>
    private bool isDefense;
    /// <summary>
    /// 每場遊戲用的速度
    /// </summary>
    private float speed;
    /// <summary>
    /// 每場遊戲用的跳躍
    /// </summary>
    private float jump;
    /// <summary>
    /// 原始速度
    /// </summary>
    private float originalSpeed;
    /// <summary>
    /// 原始跳躍
    /// </summary>
    private float originalJump;
    /// <summary>
    /// 技能按鈕：將文字列舉轉為按鈕
    /// </summary>
    private KeyCode[] skill1 = new KeyCode[3];
    /// <summary>
    /// 技能計時器
    /// </summary>
    private float timer;
    /// <summary>
    /// 招式連按次數
    /// </summary>
    private int countSkill;
    /// <summary>
    /// 每場遊戲用的血量
    /// </summary>
    private float hp;
    /// <summary>
    /// 每場遊戲用的魔力
    /// </summary>
    private float mp;
    /// <summary>
    /// 最大血量
    /// </summary>
    private float maxHp;
    /// <summary>
    /// 最大魔力
    /// </summary>
    private float maxMp;
    #endregion

    #region 事件
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        shadow = transform.Find("陰影");
        canvas = transform.Find("畫布");
        textName = canvas.Find("角色名稱").GetComponent<Text>();
        imgHp = canvas.Find("血條").GetComponent<Image>();
        imgMp = canvas.Find("魔力").GetComponent<Image>();

        originalSpeed = dataCharacter.speed;
        originalJump = dataCharacter.jump;

        speed = originalSpeed;
        jump = originalJump;

        maxHp = dataCharacter.hp;
        maxMp = dataCharacter.mp;

        hp = maxHp;
        mp = maxMp;

        SwitchSkillInput();                         // 將技能輸入文字轉為按鍵
    }

    private void Update()
    {
        Jump();
        Defense();
        Attack();
        Skill();
        FixedCanvasAngle();
        Restore(ref hp, maxHp, dataCharacter.restoreHp, imgHp);     // 恢復血量
        Restore(ref mp, maxMp, dataCharacter.restoreMp, imgMp);     // 恢復魔力
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FighterControl>() && other.gameObject != gameObject)         // 如果 碰到的物件有對戰控制器 並且非本身
        {
            other.GetComponent<FighterControl>().Damage(dataCharacter.attack);              // 對物件造成傷害
        }
    }

    /// <summary>
    /// 繪製圖示
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;                                                   // 紅色
        Gizmos.DrawRay(transform.position + rayOffset, -transform.up * length);     // 繪製射線
    }
    #endregion

    #region 方法
    /// <summary>
    /// 移動：上下左右
    /// </summary>
    private void Move()
    {
        float h = Input.GetAxis("HorizontalP" + data.index);                                        // 取得玩家左右
        float v = Input.GetAxis("VerticalP" + data.index);                                          // 取得玩家上下

        Vector3 pos = Vector3.right * h + Vector3.forward * v;                                      // 上下左右移動值

        rig.MovePosition(transform.position + pos * speed * Time.fixedDeltaTime);                   // 移動座標

        ani.SetBool("跑步開關", h != 0 || v != 0);                                                   // 跑步動畫

        if (h > 0) transform.eulerAngles = new Vector3(0, 180, 0);                                  // 按右面向右邊
        if (h < 0) transform.eulerAngles = new Vector3(0, 0, 0);                                    // 按左面向左邊
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        if (Physics.Raycast(transform.position + rayOffset, -transform.up, length, 1 << 8))     // 如果 射線 打到地板
        {
            isGround = true;                                                                    // 在地板上
            ani.SetBool("跳躍開關", !isGround);                                                  // 跳躍動畫關閉
        }

        if (Input.GetKeyDown(data.jump) && isGround)                                            // 如果 按下 跳躍 並且 在地板上
        {
            isGround = false;                                                                   // 不在地板上
            rig.AddForce(0, jump, 0);                                                           // 推力
            ani.SetBool("跳躍開關", !isGround);                                                  // 跳躍動畫開啟
        }

        Vector3 posShadow = shadow.position;                                                    // 取得陰影世界座標
        posShadow.y = 0.1f;                                                                     // 黏在地面
        posShadow.z = transform.position.z - 1.5f;                                              // Z 軸位移
        shadow.position = posShadow;                                                            // 更新陰影世界座標
    }

    /// <summary>
    /// 防禦
    /// </summary>
    private void Defense()
    {
        if (Input.GetKey(data.defence))
        {
            isDefense = true;
            ani.SetBool("防禦開關", isDefense);
            speed = 0;
        }
        else
        {
            isDefense = false;
            ani.SetBool("防禦開關", isDefense);
            speed = originalSpeed;
        }
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        if (Input.GetKeyDown(data.attack))
        {
            ani.SetTrigger("攻擊觸發");
        }
    }

    /// <summary>
    /// 招式
    /// </summary>
    private void Skill()
    {
        if ((Input.GetKeyDown(skill1[0]) || skill1[0] == data.left && Input.GetKeyDown(data.right)))                                        // 如果按下第一顆按鍵 或者 按鍵為左邊，也可接受右邊
        {
            timer = 0;                                                                                                                      // 第一顆按鍵計時器歸零
            countSkill = 1;                                                                                                                 // 連按段數：1
        }
        if ((Input.GetKeyDown(skill1[1]) || skill1[1] == data.left && Input.GetKeyDown(data.right)) && countSkill == 1)                     // 如果按下第二顆按鍵 或者 按鍵為左邊，也可接受右邊
        {
            timer = 0;                                                                                                                      // 計時器歸零
            countSkill = 2;                                                                                                                 // 連按段數：2
        }
        if ((Input.GetKeyDown(skill1[2]) || skill1[2] == data.left && Input.GetKeyDown(data.right)) && countSkill == 2)                     // 如果按下第三顆按鍵 或者 按鍵為左邊，也可接受右邊
        {
            countSkill = 3;                                                                                                                 // 連按段數：3

            if (dataCharacter.skill.cost <= mp)                                                                                             // 如果 消耗 <= 魔力
            {
                mp -= dataCharacter.skill.cost;                                                                                             // 扣除消耗
                imgMp.fillAmount = mp / maxMp;                                                                                              // 更新介面
                ani.SetTrigger("招式觸發");                                                                                                  // 觸發招式動畫
                Invoke("SkillDelaySpawn", dataCharacter.skill.interval);                                                                    // 如果為遠距離 延遲呼叫 技能延遲生成
            }
        }

        if (countSkill == 1 || countSkill == 2 || countSkill == 3)                                                                          // 連段後開始倒數 限制 按鍵只能在 interval 內達成連按
        {
            //dataCharacter.speed = 0;
            //dataCharacter.jump = 0;

            timer += Time.deltaTime;                                                                                                        // 計時器累加

            if (timer >= interval)                                                                                                          // 如果 超過 interbal 時間
            {
                timer = 0;                                                                                                                  // 計時器歸零
                countSkill = 0;                                                                                                             // 連按段數歸零

                //dataCharacter.speed = originalSpeed;
                //dataCharacter.jump = originalJump;
            }
        }
    }

    /// <summary>
    /// 技能延遲生成
    /// </summary>
    private void SkillDelaySpawn()
    {
        GameObject temp = Instantiate(dataCharacter.skill.skillObject, pointSpawn.position, Quaternion.identity);      // 生成技能物件
        temp.GetComponent<Rigidbody>().AddForce(-transform.right * dataCharacter.skill.speed);                         // 添加速度
        SkillObject skill = temp.AddComponent<SkillObject>();                                                           // 添加技能物件腳本
        skill.damage = dataCharacter.skill.damage;                                                                     // 設定傷害值
        skill.player = gameObject;                                                                                      // 設定玩家
    }

    /// <summary>
    /// 將技能輸入的上下左右攻防跳轉為輸入按鍵
    /// </summary>
    private void SwitchSkillInput()
    {
        for (int i = 0; i < dataCharacter.skill.skillInputs.Length; i++)
        {
            switch (dataCharacter.skill.skillInputs[i])
            {
                case SkillInput.上:
                    skill1[i] = data.up;
                    break;
                case SkillInput.下:
                    skill1[i] = data.down;
                    break;
                case SkillInput.前:
                    skill1[i] = data.left;
                    break;
                case SkillInput.攻:
                    skill1[i] = data.attack;
                    break;
                case SkillInput.防:
                    skill1[i] = data.defence;
                    break;
                case SkillInput.跳:
                    skill1[i] = data.jump;
                    break;
            }
        }
    }

    /// <summary>
    /// 固定畫布角度：0 0 0
    /// </summary>
    private void FixedCanvasAngle()
    {
        canvas.eulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    private void Dead()
    {
        ani.SetBool("死亡開關", true);               // 死亡動畫
        enabled = false;                            // 停止此腳本
    }

    /// <summary>
    /// 恢復
    /// </summary>
    /// <param name="value">要恢復的值</param>
    /// <param name="max">要恢復值的最大值</param>
    /// <param name="restore">每秒恢復多少值</param>
    /// <param name="bar">要更新的介面</param>
    private void Restore(ref float value, float max, float restore, Image bar)
    {
        value += restore * Time.deltaTime;
        value = Mathf.Clamp(value, 0, max);
        bar.fillAmount = value / max;
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">接收到的傷害值</param>
    public void Damage(float damage)
    {
        float d = isDefense ? damage / 2 : damage;      // 如果防禦狀態，傷害除以二
        hp -= d;                                        // 扣血
        imgHp.fillAmount = hp / maxHp;                  // 更新介面
        ani.SetTrigger("受傷觸發");                      // 受傷動畫

        if (hp <= 0) Dead();                            // 如果血量 <= 0 死亡
    }
    #endregion
}
