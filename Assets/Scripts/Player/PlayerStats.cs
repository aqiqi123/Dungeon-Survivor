using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(HealthSystem))]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {  get; private set; }

    public event Action OnPlayerStatsChanged;//通知武器更新
    public event Action OnPlayerMaxHealthChanged;

    [SerializeField] private CharacterSO characterData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed {  get; private set; }
    public float CurrentMight {  get; private set; }
    public float CurrentCooldownReduction {  get; private set; }
    public int CurrentAdditionalProjectileCount {  get; private set; }
    public float CurrentProjectileSpeed {  get; private set; }
    public float CurrentDurationMultiplier {  get; private set; }
    public float CurrentAreaMultiplier {  get; private set; }
    public int CurrentAdditionalPierceCount { get; private set; }

    private HealthSystem healthSystem;
    private PlayerMovement playerMovement;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        healthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();

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
        CurrentAdditionalPierceCount = characterData.AdditionalPierceCount;

        healthSystem.Initialize(CurrentMaxHealth);
    }

    //提供给被动道具调用的方法
    public void IncreaseMight(float amount) {
        if (amount == 0) return;
        CurrentMight += amount;
        UpdateStats();
    }

    public void IncreaseCooldownReduction(float amount) {
        if (amount == 0) return;
        CurrentCooldownReduction += amount;
        // 限制最高冷却缩减，防止减到负数或者无限开火（例如上限 90%）
        CurrentCooldownReduction = Mathf.Clamp(CurrentCooldownReduction, 0f, 0.9f);
        UpdateStats();
    }

    public void IncreaseProjectileCount(int amount) {
        if (amount == 0) return;
        CurrentAdditionalProjectileCount += amount;
        UpdateStats();
    }

    public void IncreaseMaxHealth(float amount) {
        if (amount == 0) return;
        CurrentMaxHealth += amount;
        healthSystem.IncreaseMaxHealth(amount);
        OnPlayerMaxHealthChanged?.Invoke();
    }

    public void IncreaseArea(float percentage) {
        if (percentage == 0) return;
        CurrentAreaMultiplier += percentage;
        UpdateStats();
    }

    public void IncreaseSpeed(float percentage) {
        if (percentage == 0) return;
        CurrentProjectileSpeed += percentage;
        UpdateStats();
    }

    public void IncreaseDuration(float percentage) {
        if (percentage == 0) return;
        CurrentDurationMultiplier += percentage;
        UpdateStats();
    }
    public void IncreaseMoveSpeed(float percentage) {
        if (percentage == 0) return;
        CurrentMoveSpeed += percentage;
        UpdateStats();
    }

    public void IncreasePierceCount(int amount) {
        if (amount == 0) return;
        CurrentAdditionalPierceCount += amount;
        UpdateStats();
    }

    public void ApplyPassiveItem(PassiveItemSO item) {
        if (item == null) return;

        IncreaseMight(item.MightBonus);
        IncreaseCooldownReduction(item.CooldownReductionBonus);
        IncreaseMoveSpeed(item.MoveSpeedBonus);
        IncreaseArea(item.AreaBonus);
        IncreaseSpeed(item.SpeedBonus);
        IncreaseDuration(item.DurationBonus);
        IncreaseProjectileCount(item.AmountBonus);
        IncreasePierceCount(item.PierceBonus);
        IncreaseMaxHealth(item.MaxHealthBonus);
    }

    private void UpdateStats() {
        //玩家属性改变，就通知所有武器更新
        OnPlayerStatsChanged?.Invoke();
    }

    public Vector2 GetFacingDirection() {
        return playerMovement.facingDirection;
    }
}
