using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterSO",menuName ="SOs/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float might;
    [SerializeField] private float cooldownReduction;
    [SerializeField] private int additionalProjectileCount;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float durationMultiplier;
    [SerializeField] private float areaMultiplier;

    public float MaxHealth=>maxHealth;
    public float MoveSpeed=> moveSpeed;
    public float Might => might;
    public float CooldownReduction => cooldownReduction;
    public int AdditionalProjectileCount => additionalProjectileCount;
    public float ProjectileSpeed => projectileSpeed;
    public float DurationMultiplier => durationMultiplier;
    public float AreaMultiplier => areaMultiplier;
}
