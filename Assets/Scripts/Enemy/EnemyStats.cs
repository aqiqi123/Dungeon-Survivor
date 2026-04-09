using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyStats : PoolableObject
{
    [SerializeField] EnemySO enemyData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentDamage {  get; private set; }
    public float CurrentSizeMultiplier { get; private set; } = 1f;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void OnSpawn() {
        InitializeStats();
        transform.localScale = Vector3.one;
        CurrentSizeMultiplier = 1f;

        healthSystem.OnDeath+= HandleDeath;

        if (EnemySpatialManager.Instance != null && TryGetComponent<EnemyAgent>(out var agent))
        {
            EnemySpatialManager.Instance.Register(agent);
        }
    }

    public override void OnDespawn() {
        healthSystem.OnDeath -= HandleDeath;

        if (EnemySpatialManager.Instance != null && TryGetComponent<EnemyAgent>(out var agent))
        {
            EnemySpatialManager.Instance.UnRegister(agent);
        }
    }

    private void InitializeStats() {
        if (enemyData == null) {
            Debug.LogError("PlayerStats:忘记拖拽CharacterSO了！");
            return;
        }

        CurrentMaxHealth = enemyData.MaxHealth;
        CurrentMoveSpeed = enemyData.MoveSpeed;
        CurrentDamage = enemyData.Damage;

        healthSystem.Initialize(CurrentMaxHealth);
    }

    // 处理敌人死亡
    private void HandleDeath() {
        if (EnemySpawner.Instance != null) {
            EnemySpawner.Instance.OnEnemyDespawned();
        }
        ReturnToPool();
    }

    public void ApplyBuffs(int loopCount, float healthGrowth, float healthLimit,
                           float speedGrowth, float speedLimit,
                           float damageGrowth, float damageLimit,
                           float sizeGrowth, float sizeLimit) {
        if (loopCount <= 0) return;

        float hpMult = 1f + (loopCount * healthGrowth);
        float speedMult = 1f + (loopCount * speedGrowth);
        float dmgMult = 1f + (loopCount * damageGrowth);
        float sizeMult = 1f + (loopCount * sizeGrowth);

        hpMult = Mathf.Min(hpMult, healthLimit);
        speedMult = Mathf.Min(speedMult, speedLimit);
        dmgMult = Mathf.Min(dmgMult, damageLimit);
        sizeMult = Mathf.Min(sizeMult, sizeLimit);

        CurrentMaxHealth *= hpMult;
        CurrentMoveSpeed *= speedMult;
        CurrentDamage *= dmgMult;
        CurrentSizeMultiplier = sizeMult;

        transform.localScale = Vector3.one * sizeMult;

        healthSystem.Initialize(CurrentMaxHealth);
    }
}
