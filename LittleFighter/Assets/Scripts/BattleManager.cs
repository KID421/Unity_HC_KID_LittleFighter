using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("玩家資料")]
    public ControlData[] players;
    [Header("結束畫面")]
    public Transform panelFinal;

    /// <summary>
    /// 對戰控制器
    /// </summary>
    private FighterControl[] fcs = new FighterControl[2];

    private void Awake()
    {
        SpawnPlayer();
    }

    private void OnDead(int playerIndex)
    {
        StartCoroutine(ShowFinalPanel(playerIndex));
    }

    private IEnumerator ShowFinalPanel(int playerIndex)
    {
        Image img = panelFinal.GetComponent<Image>();

        int win = playerIndex == 1 ? 2 : 1;
        panelFinal.Find("訊息").GetComponent<Text>().text = "恭喜【玩家 " + win + " 】獲勝";

        while (img.color.a < 0.8f)
        {
            img.color += new Color(0, 0, 0, 0.1f);
            yield return null;
        }

        for (int i = 0; i < panelFinal.childCount; i++)
        {
            if (panelFinal.GetChild(i).GetComponent<Image>())
            {
                Image imgChild = panelFinal.GetChild(i).GetComponent<Image>();

                while (imgChild.color.a < 1)
                {
                    imgChild.color += new Color(0, 0, 0, 0.1f);
                    yield return null;
                }
            }
            else if (panelFinal.GetChild(i).GetComponent<Text>())
            {
                Text textChild = panelFinal.GetChild(i).GetComponent<Text>();

                while (textChild.color.a < 1)
                {
                    textChild.color += new Color(0, 0, 0, 0.1f);
                    yield return null;
                }
            }
        }
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
            fcs[i] = fc;
            fc.data = players[i];                                                                                           // 指定生出來的物件玩家資料
        }

        for (int i = 0; i < fcs.Length; i++)
        {
            fcs[i].onDead += OnDead;
        }
    }
}
