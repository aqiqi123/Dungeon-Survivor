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
    public event Action OnPlayerHealthChanged;
    public event Action OnGoldChanged;

    [SerializeField] private CharacterSO characterData;
    public CharacterSO CharacterData => characterData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed {  get; private set; }
    public float CurrentMight {  get; private set; }
    public float CurrentMagnet {  get; private set; }
    public float CurrentCooldownReduction {  get; private set; }
    public int CurrentAdditionalProjectileCount {  get; private set; }
    public float CurrentProjectileSpeed {  get; private set; }
    public float CurrentDurationMultiplier {  get; private set; }
    public float CurrentAreaMultiplier {  get; private set; }
    public int CurrentAdditionalPierceCount { get; private set; }
    public int CurrentGold { get; private set; }

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
        CurrentMagnet = characterData.Magnet;
        CurrentCooldownReduction = characterData.CooldownReduction;
        CurrentAdditionalProjectileCount = characterData.AdditionalProjectileCount;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentDurationMultiplier = characterData.DurationMultiplier;
        CurrentAreaMultiplier = characterData.AreaMultiplier;
        CurrentAdditionalPierceCount = characterData.AdditionalPierceCount;

        healthSystem.Initialize(CurrentMaxHealth);
    }

    //提供给被动道具调用的方法
    public void IncreaseMight(float amount) { if (amount == 0) return; CurrentMight += amount; UpdateStats(); }
    public void IncreaseMagnet(float percentage) { if (percentage == 0) return; CurrentMagnet *= (1 + percentage); UpdateStats(); }
    public void IncreaseCooldownReduction(float amount) { if (amount == 0) return; CurrentCooldownReduction += amount; CurrentCooldownReduction = Mathf.Clamp(CurrentCooldownReduction, 0f, 0.9f); UpdateStats(); }
    public void IncreaseProjectileCount(int amount) { if (amount == 0) return; CurrentAdditionalProjectileCount += amount; UpdateStats(); }
    public void IncreaseMaxHealth(float amount) { if (amount == 0) return; CurrentMaxHealth += amount; healthSystem.IncreaseMaxHealth(amount); OnPlayerMaxHealthChanged?.Invoke(); }
    public void IncreaseArea(float percentage) { if (percentage == 0) return; CurrentAreaMultiplier *=(1+ percentage); UpdateStats(); }
    public void IncreaseSpeed(float percentage) { if (percentage == 0) return; CurrentProjectileSpeed += percentage; UpdateStats(); }
    public void IncreaseDuration(float percentage) { if (percentage == 0) return; CurrentDurationMultiplier += percentage; UpdateStats(); }
    public void IncreaseMoveSpeed(float percentage) { if (percentage == 0) return; CurrentMoveSpeed *=(1+ percentage); UpdateStats(); }
    public void IncreasePierceCount(int amount) { if (amount == 0) return; CurrentAdditionalPierceCount += amount; UpdateStats(); }
    public void Heal(float amount) { healthSystem.Heal(amount); OnPlayerHealthChanged?.Invoke(); }


    public void ApplyPassiveItem(PassiveItemSO item) {
        if (item == null || item.LevelData == null || item.LevelData.Count == 0) return;

        // 默认应用第一级 (Index 0)
        ApplyPassiveStats(item.LevelData[0]);
    }

    // 这个方法会被 UpgradeManager 调用
    public void ApplyPassiveStats(PassiveStats stats) {
        IncreaseMight(stats.mightBonus);
        IncreaseMagnet(stats.magnetBonus);
        IncreaseCooldownReduction(stats.cooldownReductionBonus);
        IncreaseMoveSpeed(stats.moveSpeedBonus);
        IncreaseArea(stats.areaBonus);
        IncreaseSpeed(stats.speedBonus);
        IncreaseDuration(stats.durationBonus);
        IncreaseProjectileCount(stats.amountBonus);
        IncreasePierceCount(stats.pierceBonus);
        IncreaseMaxHealth(stats.maxHealthBonus);
    }

    public void AddGold(int amount) {
        CurrentGold += amount;
        OnGoldChanged?.Invoke();
    }

    private void UpdateStats() {
        OnPlayerStatsChanged?.Invoke();
    }

    public Vector2 GetFacingDirection() {
        return playerMovement.facingDirection;
    }
}
