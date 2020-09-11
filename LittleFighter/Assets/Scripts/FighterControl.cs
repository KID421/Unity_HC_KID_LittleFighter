using System.Xml;
using UnityEngine;

/// <summary>
/// 對戰控制系統：移動、攻防跳與動畫
/// </summary>
public class FighterControl : MonoBehaviour
{
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

    private Rigidbody rig;
    private Animator ani;
    private bool isGround;
    private bool isDefense;
    private float originalSpeed;
    private float originalJump;
    private KeyCode[] skill1 = new KeyCode[3];
    private float timer;
    private int countSkill;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        originalSpeed = dataCharacter.speed;
        originalJump = dataCharacter.jump;

        SwitchSkillInput();
    }

    private void Update()
    {
        Jump();
        Defense();
        Attack();
        Skill();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 移動：上下左右
    /// </summary>
    private void Move()
    {
        float h = Input.GetAxis("HorizontalP" + data.index);                                        // 取得玩家左右
        float v = Input.GetAxis("VerticalP" + data.index);                                          // 取得玩家上下

        Vector3 pos = Vector3.right * h + Vector3.forward * v;                                      // 上下左右移動值

        rig.MovePosition(transform.position + pos * dataCharacter.speed * Time.fixedDeltaTime);     // 移動座標

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
            rig.AddForce(0, dataCharacter.jump, 0);                                             // 推力
            ani.SetBool("跳躍開關", !isGround);                                                  // 跳躍動畫開啟
        }
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
            dataCharacter.speed = 0;
        }
        else
        {
            isDefense = false;
            ani.SetBool("防禦開關", isDefense);
            dataCharacter.speed = originalSpeed;
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
        if ((Input.GetKeyDown(skill1[0]) || skill1[0] == data.left && Input.GetKeyDown(data.right)))
        {
            timer = 0;
            countSkill = 1;
        }
        if ((Input.GetKeyDown(skill1[1]) || skill1[1] == data.left && Input.GetKeyDown(data.right)) && countSkill == 1)
        {
            timer = 0;
            countSkill = 2;
        }
        if ((Input.GetKeyDown(skill1[2]) || skill1[2] == data.left && Input.GetKeyDown(data.right)) && countSkill == 2)
        {
            countSkill = 3;
            ani.SetTrigger("招式 1 觸發");

            if (dataCharacter.skill1.skillType == SkillType.遠距離) Invoke("SkillDelaySpawn", dataCharacter.skill1.interval);
        }

        if (countSkill == 1 || countSkill == 2 || countSkill == 3)
        {
            //dataCharacter.speed = 0;
            //dataCharacter.jump = 0;

            timer += Time.deltaTime;

            if (timer >= interval)
            {
                timer = 0;
                countSkill = 0;

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
        GameObject temp = Instantiate(dataCharacter.skill1.skillObject, pointSpawn.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(-transform.right * dataCharacter.skill1.speed);
    }

    /// <summary>
    /// 將技能輸入的上下左右攻防跳轉為輸入按鍵
    /// </summary>
    private void SwitchSkillInput()
    {
        for (int i = 0; i < dataCharacter.skill1.skillInputs.Length; i++)
        {
            switch (dataCharacter.skill1.skillInputs[i])
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
    /// 繪製圖示
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;                                                   // 紅色
        Gizmos.DrawRay(transform.position + rayOffset, -transform.up * length);     // 繪製射線
    }
}
