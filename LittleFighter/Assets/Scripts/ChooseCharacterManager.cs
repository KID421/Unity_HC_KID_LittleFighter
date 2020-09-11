using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterManager : MonoBehaviour
{
    [Header("玩家資料")]
    public ControlData[] players;
    [Header("角色圖像")]
    public Image[] imgCharacters;
    [Header("確認音效")]
    public AudioClip soundOK;
    [Header("所有角色資料")]
    public CharacterData[] characters;

    /// <summary>
    /// 選取角色編號，預設玩家 1 為 0，玩家 2 為 1
    /// </summary>
    private int[] indexes = { 0, 1 };

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();

        for (int i = 0; i < players.Length; i++) players[i].chooseCharacter = false;        // 遊戲一開始設定為尚未選取
    }

    private void Update()
    {
        ChooseCharacter();
    }

    /// <summary>
    /// 選取角色
    /// </summary>
    private void ChooseCharacter()
    {
        if (!Input.anyKeyDown) return;                                              // 如果沒有按任意鍵 跳出

        for (int i = 0; i < players.Length; i++)
        {
            if (Input.GetKeyDown(players[i].left) && !players[i].chooseCharacter)   // 按下左 並且 尚未選取
            {
                indexes[i]--;                                                       // 編號遞減

                if (indexes[i] == -1) indexes[i] = 1;                               // 如果 -1 就改回 最大值 1
            }
            if (Input.GetKeyDown(players[i].right) && !players[i].chooseCharacter)  // 按下右 並且 尚未選取
            {
                indexes[i]++;                                                       // 編號遞增

                if (indexes[i] == 2) indexes[i] = 0;                                // 如果 最大值 就改回 0
            }

            imgCharacters[i].sprite = characters[indexes[i]].sprite;                // 更新角色圖像

            if (Input.GetKeyDown(players[i].attack) && !players[i].chooseCharacter) // 如果按下攻擊 並且 尚未選取
            {
                players[i].chooseCharacter = true;                                  // 已經選取
                aud.pitch = 1;                                                      // 音調正常
                aud.PlayOneShot(soundOK);                                           // 音效
                imgCharacters[i].color = new Color(1, 1, 1, 0.5f);                  // 圖像半透明代表選取
                players[i].dataCharacter = characters[indexes[i]];                  // 設定角色選取的角色資料
            }

            if (Input.GetKeyDown(players[i].defence) && players[i].chooseCharacter) // 如果按下防禦 並且 已經選取
            {
                players[i].chooseCharacter = false;                                 // 尚未選取
                aud.pitch = 0.8f;                                                   // 音調低
                aud.PlayOneShot(soundOK);                                           // 音效
                imgCharacters[i].color = new Color(1, 1, 1, 1f);                    // 圖像全顯示代表尚未選取
            }
        }
    }
}
