using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 載入場景並且淡入
/// 按鈕：要啟動載入場景的按鈕
/// </summary>
public class LoadSceneAndFade : MonoBehaviour
{
    [Header("黑畫面")]
    public Image imgBlack;
    [Header("要載入的場景名稱")]
    public string nameScene;

    private void Awake()
    {
        //if (SceneManager.GetActiveScene().name == "選單") Screen.SetResolution(1280, 720, false);
    }

    /// <summary>
    /// 按鈕：要進入下一個場景的按鈕
    /// </summary>
    public void StartGame(string name)
    {
        StartCoroutine(DelayStartGame(name));
    }

    /// <summary>
    /// 延遲開始遊戲：讓音效播完
    /// </summary>
    private IEnumerator DelayStartGame(string name)
    {
        Color color = imgBlack.color;

        while (color.a < 1)
        {
            color.a += 0.05f;
            imgBlack.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);

        if (name != "") nameScene = name;

        SceneManager.LoadScene(nameScene);
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quit()
    {
        Invoke("DelayQuit", 2f);
    }

    private void DelayQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 開啟連結
    /// </summary>
    /// <param name="web">網址</param>
    public void OpenLink(string web)
    {
        Application.OpenURL(web);
    }
}
