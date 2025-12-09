using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemSO", menuName = "SOs/PassiveItem")]
public class PassiveItemSO : ScriptableObject
{
    [Header("UI信息")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    [Header("属性加成（填0代表不修改）")]
    [Tooltip("例如 0.1 代表增加 10% 伤害")][SerializeField] private float mightBonus;
    [Tooltip("例如 0.05 代表减少 5% 冷却")][SerializeField] private float cooldownReductionBonus;
    [Tooltip("例如 0.1 代表增加 10% 移动速度")][SerializeField] private float moveSpeedBonus;
    [Tooltip("例如 10 代表增加 10点 生命上限")][SerializeField] private float maxHealthBonus;
    [Tooltip("例如 0.1 代表增加 10% 范围")][SerializeField] private float areaBonus;
    [Tooltip("例如 0.1 代表增加 10% 子弹飞行速度")][SerializeField] private float speedBonus;
    [Tooltip("例如 0.1 代表增加 10% 持续时间")][SerializeField]private float durationBonus;
    [Tooltip("增加投射物数量 (整数)")][SerializeField]private int amountBonus;
    [SerializeField]private int pierceBonus;

    public string ItemName=>itemName;
    public Sprite Icon=>icon;
    public float MightBonus => mightBonus;
    public float CooldownReductionBonus => cooldownReductionBonus;
    public float MoveSpeedBonus => moveSpeedBonus;
    public float MaxHealthBonus => maxHealthBonus;
    public float AreaBonus => areaBonus;
    public float SpeedBonus => speedBonus;
    public float DurationBonus => durationBonus;
    public int AmountBonus => amountBonus;
    public int PierceBonus => pierceBonus;
}
