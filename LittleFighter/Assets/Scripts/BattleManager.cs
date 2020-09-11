using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("玩家資料")]
    public ControlData[] players;

    private void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 pos = new Vector3(i == 0 ? -10 : 10, 0.5f, 0);
            Quaternion angle = Quaternion.Euler(0, i == 0 ? 180 : 0, 0);
            FighterControl fc = Instantiate(players[i].dataCharacter.prefab, pos , angle).GetComponent<FighterControl>();
            fc.data = players[i];
        }
    }
}
