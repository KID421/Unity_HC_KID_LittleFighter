using UnityEngine;

/// <summary>
/// 控制資料：上下左右攻防跳，七顆按鈕
/// </summary>
[CreateAssetMenu(fileName = "控制資料", menuName = "KID/控制資料")]
public class ControlData : ScriptableObject
{
    [Header("上")]
    public KeyCode up;
    [Header("下")]
    public KeyCode down;
    [Header("左")]
    public KeyCode left;
    [Header("右")]
    public KeyCode right;
    [Header("攻")]
    public KeyCode attack;
    [Header("防")]
    public KeyCode defence;
    [Header("跳")]
    public KeyCode jump;
    [Header("角色")]
    public string character;
    [Header("確認選取角色")]
    public bool chooseCharacter;
    [Header("玩家編號")]
    public int index;
}