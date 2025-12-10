using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterSO",menuName ="SOs/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite icon;
    [Space]
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    [Tooltip("例如 2 代表武器有双倍伤害")][SerializeField] private float might;
    [SerializeField] private float magnet;
    [Tooltip("例如 0.05 代表减少 5% 冷却")][SerializeField] private float cooldownReduction;
    [SerializeField] private int additionalProjectileCount;
    [Tooltip("例如 1.1 代表增加 10% 子弹飞行速度")][SerializeField] private float projectileSpeed;
    [SerializeField] private float durationMultiplier;
    [SerializeField] private float areaMultiplier;
    [SerializeField] private int additionalPierceCount;

    public string CharacterName=>characterName;
    public Sprite Icon=>icon;
    public float MaxHealth=>maxHealth;
    public float MoveSpeed=> moveSpeed;
    public float Might => might;
    public float Magnet => magnet;
    public float CooldownReduction => cooldownReduction;
    public int AdditionalProjectileCount => additionalProjectileCount;
    public float ProjectileSpeed => projectileSpeed;
    public float DurationMultiplier => durationMultiplier;
    public float AreaMultiplier => areaMultiplier;
    public int AdditionalPierceCount=>additionalPierceCount;
}
