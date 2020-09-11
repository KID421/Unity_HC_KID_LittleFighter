using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("玩家資料")]
    public ControlData[] players;

    private void Awake()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// 生成玩家物件
    /// </summary>
    private void SpawnPlayer()
    {
        // 迴圈執行有幾位玩家
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 pos = new Vector3(i == 0 ? -10 : 10, 0.5f, 0);                                                          // 如果 編號 0 左邊 否則 右邊
            Quaternion angle = Quaternion.Euler(0, i == 0 ? 180 : 0, 0);                                                    // 如果 編號 0 面向右 否則 面向左
            FighterControl fc = Instantiate(players[i].dataCharacter.prefab, pos, angle).GetComponent<FighterControl>();    // 生成玩家物件
            fc.data = players[i];                                                                                           // 指定生出來的物件玩家資料
        }
    }
}
