using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {  get; private set; }

    public event Action OnPlayerStatsChanged;//通知武器更新
    public event Action OnPlayerMaxHealthChanged;
    public event Action OnPlayerHealthChanged;
    public event Action OnGoldChanged;

    private CharacterSO characterData;
    public CharacterSO CharacterData => model != null ? model.CharacterData : characterData;

    public float CurrentMaxHealth => model.CurrentMaxHealth;
    public float CurrentMoveSpeed => model.CurrentMoveSpeed;
    public float CurrentMight => model.CurrentMight;
    public float CurrentMagnet => model.CurrentMagnet;
    public float CurrentCooldownReduction => model.CurrentCooldownReduction;
    public int CurrentAdditionalProjectileCount => model.CurrentAdditionalProjectileCount;
    public float CurrentProjectileSpeed => model.CurrentProjectileSpeed;
    public float CurrentDurationMultiplier => model.CurrentDurationMultiplier;
    public float CurrentAreaMultiplier => model.CurrentAreaMultiplier;
    public int CurrentAdditionalPierceCount => model.CurrentAdditionalPierceCount;
    public int CurrentGold => model.CurrentGold;
    public float CurrentLuck => model.CurrentLuck;

    public PlayerModel Model => model;

    private HealthSystem healthSystem;
    private PlayerMovement playerMovement;
    private PlayerModel model;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (CharacterManager.SelectedCharacter != null) {
            characterData= CharacterManager.SelectedCharacter;
        }

        healthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.SetVisual(characterData.Icon);

        InitializeStats();
    }

    private void InitializeStats() {
        model = new PlayerModel();
        model.Initialize(characterData);

        healthSystem.Initialize(model.CurrentMaxHealth);
    }

    //提供给被动道具调用的方法
    public void IncreaseMight(float amount) { if (!model.IncreaseMight(amount)) return; UpdateStats(); }
    public void IncreaseMagnet(float percentage) { if (!model.IncreaseMagnet(percentage)) return; UpdateStats(); }
    public void IncreaseCooldownReduction(float amount) { if (!model.IncreaseCooldownReduction(amount)) return; UpdateStats(); }
    public void IncreaseProjectileCount(int amount) { if (!model.IncreaseProjectileCount(amount)) return; UpdateStats(); }
    public void IncreaseMaxHealth(float amount) { if (!model.IncreaseMaxHealth(amount)) return; healthSystem.IncreaseMaxHealth(amount); OnPlayerMaxHealthChanged?.Invoke(); }
    public void IncreaseArea(float percentage) { if (!model.IncreaseArea(percentage)) return; UpdateStats(); }
    public void IncreaseSpeed(float percentage) { if (!model.IncreaseSpeed(percentage)) return; UpdateStats(); }
    public void IncreaseDuration(float percentage) { if (!model.IncreaseDuration(percentage)) return; UpdateStats(); }
    public void IncreaseMoveSpeed(float percentage) { if (!model.IncreaseMoveSpeed(percentage)) return; UpdateStats(); }
    public void IncreasePierceCount(int amount) { if (!model.IncreasePierceCount(amount)) return; UpdateStats(); }
    public void IncreaseLuck(float amount) { if (!model.IncreaseLuck(amount)) return; UpdateStats(); }
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
        IncreaseLuck(stats.luckBonus);
    }

    public void AddGold(int amount) {
        model.AddGold(amount);
        OnGoldChanged?.Invoke();
    }

    private void UpdateStats() {
        OnPlayerStatsChanged?.Invoke();
    }

    public Vector2 GetFacingDirection() {
        return model.FacingDirection;
    }
}
