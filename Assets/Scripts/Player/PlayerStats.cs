using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {  get; private set; }

    public event Action OnPlayerStatsChanged;

    [SerializeField] private CharacterSO characterData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed {  get; private set; }
    public float CurrentMight {  get; private set; }
    public float CurrentCooldownReduction {  get; private set; }
    public int CurrentAdditionalProjectileCount {  get; private set; }
    public float CurrentProjectileSpeed {  get; private set; }
    public float CurrentDurationMultiplier {  get; private set; }
    public float CurrentAreaMultiplier {  get; private set; }

    private HealthSystem healthSystem;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        healthSystem = GetComponent<HealthSystem>();

        InitializeStats();
    }

    private void InitializeStats() {
        if(characterData == null) {
            Debug.LogError("PlayerStats:忘记拖拽CharacterSO了！");
            return;
        }

        CurrentMaxHealth = characterData.MaxHealth;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentCooldownReduction = characterData.CooldownReduction;
        CurrentAdditionalProjectileCount = characterData.AdditionalProjectileCount;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentDurationMultiplier = characterData.DurationMultiplier;
        CurrentAreaMultiplier = characterData.AreaMultiplier;

        healthSystem.Initialize(CurrentMaxHealth);
    }

    //提供给被动道具调用的方法

    public void IncreaseMight(float amount) {
        CurrentMight += amount;
        UpdateStats();
    }

    public void IncreaseCooldownReduction(float amount) {
        CurrentCooldownReduction += amount;
        // 限制最高冷却缩减，防止减到负数或者无限开火（例如上限 90%）
        CurrentCooldownReduction = Mathf.Clamp(CurrentCooldownReduction, 0f, 0.9f);
        UpdateStats();
    }

    public void IncreaseProjectileCount(int amount) {
        CurrentAdditionalProjectileCount += amount;
        UpdateStats();
    }

    public void IncreaseArea(float percentage) {
        CurrentAreaMultiplier += percentage;
        UpdateStats();
    }

    public void IncreaseSpeed(float percentage) {
        CurrentProjectileSpeed += percentage;
        UpdateStats();
    }

    public void IncreaseDuration(float percentage) {
        CurrentDurationMultiplier += percentage;
        UpdateStats();
    }

    public void ApplyPassiveItem(PassiveItemSO item) {
        if (item == null) return;

        // 1. 逐个应用属性

        CurrentMight += item.MightBonus;

        // 冷却缩减有上限（比如 90%），防止无限发射
        CurrentCooldownReduction += item.CooldownReductionBonus;
        CurrentCooldownReduction = Mathf.Clamp(CurrentCooldownReduction, 0f, 0.9f);

        CurrentMoveSpeed += (characterData.MoveSpeed * item.MoveSpeedBonus); // 移速是基于基础值的百分比加成

        CurrentAreaMultiplier += item.AreaBonus;
        CurrentProjectileSpeed += item.SpeedBonus;
        CurrentDurationMultiplier += item.DurationBonus;
        CurrentAdditionalProjectileCount += item.AmountBonus;

        // 特殊处理：最大生命值
        if (item.MaxHealthBonus > 0) {
            CurrentMaxHealth += item.MaxHealthBonus;
            // 增加上限的同时，通常也会给玩家回一口血，或者让 HealthSystem 重新计算百分比
            // 这里我们需要通知 HealthSystem 更新上限
            healthSystem.IncreaseMaxHealth(item.MaxHealthBonus);
        }

        UpdateStats();
    }

    private void UpdateStats() {
        //玩家属性改变，就通知所有武器更新
        OnPlayerStatsChanged?.Invoke();
    }
}
