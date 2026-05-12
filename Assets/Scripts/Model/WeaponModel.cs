using System.Collections;
using UnityEngine;

public class WeaponModel
{
    public WeaponSO WeaponData { get; private set; }

    public GameObject CurrentBulletPrefab { get; private set; }
    public float CurrentDamage { get; private set; }
    public float CurrentCooldown { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float CurrentDuration { get; private set; }
    public int CurrentCount { get; private set; }
    public float CurrentArea { get; private set; }
    public int CurrentPierce { get; private set; }
    public float CurrentProjectileInterval { get; private set; }
    public float CurrentAttackInterval { get; private set; }

    private int currentLevelIndex = 0;

    public void Initialize(WeaponSO data) {
        WeaponData = data;
        currentLevelIndex = 0;
    }

    public void LevelUp(int newLevel) {
        if (WeaponData != null && newLevel <= WeaponData.LevelData.Count) {
            currentLevelIndex = newLevel - 1;
        }
    }

    public bool RecalculateStats(PlayerModel playerModel) {
        if (WeaponData == null || playerModel == null) return false;

        if (WeaponData.LevelData == null || WeaponData.LevelData.Count == 0) {
            Debug.LogError($"WeaponSO {WeaponData.name} ȱ�ٵȼ����ݣ�");
            return false;
        }

        if (currentLevelIndex >= WeaponData.LevelData.Count) {
            currentLevelIndex = WeaponData.LevelData.Count - 1;
        }

        WeaponStats stats = WeaponData.LevelData[currentLevelIndex];

        CurrentBulletPrefab = WeaponData.BulletPrefab;
        CurrentDamage = stats.damage * playerModel.CurrentMight;
        CurrentCooldown = stats.cooldown * (1f - playerModel.CurrentCooldownReduction);
        CurrentCount = stats.count + playerModel.CurrentAdditionalProjectileCount;
        CurrentArea = stats.area * playerModel.CurrentAreaMultiplier;
        CurrentSpeed = stats.speed * playerModel.CurrentProjectileSpeed;
        CurrentDuration = stats.duration * playerModel.CurrentDurationMultiplier;
        CurrentPierce = stats.pierce + playerModel.CurrentAdditionalPierceCount;
        CurrentProjectileInterval = stats.projectileInterval;
        CurrentAttackInterval = stats.attackInterval;

        return true;
    }
}
