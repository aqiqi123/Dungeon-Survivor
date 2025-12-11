using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PassiveStats {
    [TextArea] public string description;
    public float mightBonus;
    public float magnetBonus;
    public float cooldownReductionBonus;
    public float moveSpeedBonus;
    public float maxHealthBonus;
    public float areaBonus;
    public float speedBonus;
    public float durationBonus;
    public int amountBonus;
    public int pierceBonus;
}

[CreateAssetMenu(fileName = "PassiveItemSO", menuName = "SOs/PassiveItem")]
public class PassiveItemSO : ScriptableObject
{
    [Header("UI信息")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    [Header("等级数据")]
    [SerializeField] private List<PassiveStats> levelData;

    public string ItemName=>itemName;
    public Sprite Icon=>icon;
    public List<PassiveStats> LevelData=>levelData;
    public int MaxLevel => levelData.Count;
}
