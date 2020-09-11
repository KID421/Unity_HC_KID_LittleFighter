using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 選單管理器
/// 開始遊戲按鈕：進入選角色場景
/// 控制設定按鈕：顯示控制畫面
/// </summary>
public class MenuManager : MonoBehaviour
{
    [Header("黑畫面")]
    public Image imgBlack;

    /// <summary>
    /// 開始遊戲按鈕：進入選角色場景
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(DelayStartGame());
    }

    /// <summary>
    /// 延遲開始遊戲：讓音效播完
    /// </summary>
    private IEnumerator DelayStartGame()
    {
        Color color = imgBlack.color;

        while (color.a < 1)
        {
            color.a += 0.05f;
            imgBlack.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("選角色");
    }
}
