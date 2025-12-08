using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public WeaponSO weaponData {  get; private set; }

    public GameObject CurrentBulletPrefab { get; private set; }
    public float CurrentDamage {  get; private set; }
    public float CurrentCooldown { get; private set; }  
    public float CurrentSpeed { get; private set; }
    public float CurrentDuration { get; private set; }
    public int CurrentCount { get; private set; }
    public float CurrentArea { get; private set; }

    protected float timer;

    public void Initialize(WeaponSO data) {
        weaponData = data;

        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged += RecalculateStats;
        }

        RecalculateStats(); // 初始算一次
    }

    protected virtual void OnDestroy() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged -= RecalculateStats;
        }
    }

    public void RecalculateStats() {
        if (weaponData == null || PlayerStats.Instance == null) return;

        CurrentBulletPrefab = weaponData.BulletPrefab;

        // 伤害 = 基础伤害 * 玩家力量系数 (Might)
        CurrentDamage = weaponData.Damage * PlayerStats.Instance.CurrentMight;

        // 冷却 = 基础冷却 * (1 - 冷却缩减率)
        // 例如：1秒冷却，缩减0.1(10%) -> 1 * 0.9 = 0.9秒
        CurrentCooldown = weaponData.Cooldown * (1f - PlayerStats.Instance.CurrentCooldownReduction);

        // 数量 = 基础数量 + 玩家额外数量
        CurrentCount = weaponData.Count + (int)PlayerStats.Instance.CurrentAdditionalProjectileCount;

        // 范围 = 基础范围 * 范围加成
        CurrentArea = weaponData.Area * PlayerStats.Instance.CurrentAreaMultiplier;

        // 速度 = 基础速度 * 速度加成
        CurrentSpeed = weaponData.Speed * PlayerStats.Instance.CurrentProjectileSpeed;

        // 持续时间 = 基础时间 * 持续时间加成
        CurrentDuration = weaponData.Duration * PlayerStats.Instance.CurrentDurationMultiplier;

        // 如果是光环类武器，这里可能需要更新一下 transform.localScale 来立刻反映范围变化
        // 调用一个新方法，通知子类数值变了
        OnStatsUpdated();
    }

    protected virtual void Update() {
        timer-= Time.deltaTime;
        if (timer < 0) {
            Attack();
            timer=CurrentCooldown;
        }
    }

    // 供子类重写的空方法
    protected virtual void OnStatsUpdated() { }

    protected abstract void Attack();
}
