using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyStats : PoolableObject
{
    [SerializeField] EnemySO enemyData;

    public float CurrentMaxHealth => model.CurrentMaxHealth;
    public float CurrentMoveSpeed => model.CurrentMoveSpeed;
    public float CurrentDamage => model.CurrentDamage;
    public float CurrentSizeMultiplier => model.CurrentSizeMultiplier;

    private HealthSystem healthSystem;
    private EnemyModel model;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void OnSpawn() {
        InitializeStats();
        transform.localScale = Vector3.one * model.CurrentSizeMultiplier;

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

        model = new EnemyModel();
        model.Initialize(enemyData);

        healthSystem.Initialize(model.CurrentMaxHealth);
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
        if (!model.ApplyBuffs(loopCount, healthGrowth, healthLimit, speedGrowth, speedLimit, damageGrowth, damageLimit, sizeGrowth, sizeLimit)) {
            return;
        }

        transform.localScale = Vector3.one * model.CurrentSizeMultiplier;

        healthSystem.Initialize(model.CurrentMaxHealth);
    }
}
