using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyStats : PoolableObject
{
    [SerializeField] EnemySO enemyData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentDamage {  get; private set; }

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    public override void OnSpawn() {
        InitializeStats();

        healthSystem.OnDeath+= HandleDeath;
    }

    public override void OnDespawn() {
        healthSystem.OnDeath -= HandleDeath;
    }

    private void InitializeStats() {
        if (enemyData == null) {
            Debug.LogError("PlayerStats:Íü¼ÇÍÏ×§CharacterSOÁË£¡");
            return;
        }

        CurrentMaxHealth = enemyData.MaxHealth;
        CurrentMoveSpeed = enemyData.MoveSpeed;
        CurrentDamage = enemyData.Damage;

        healthSystem.Initialize(CurrentMaxHealth);
    }

    private void HandleDeath() {
        ReturnToPool();
    }

    public void ApplyBuffs(int loopCount, float healthGrowth, float healthLimit,
                           float speedGrowth, float speedLimit,
                           float damageGrowth, float damageLimit) {
        if (loopCount <= 0) return;

        float hpMult = 1f + (loopCount * healthGrowth);
        float speedMult = 1f + (loopCount * speedGrowth);
        float dmgMult = 1f + (loopCount * damageGrowth);

        hpMult = Mathf.Min(hpMult, healthLimit);
        speedMult = Mathf.Min(speedMult, speedLimit);
        dmgMult = Mathf.Min(dmgMult, damageLimit);

        CurrentMaxHealth *= hpMult;
        CurrentMoveSpeed *= speedMult;
        CurrentDamage *= dmgMult;

        healthSystem.Initialize(CurrentMaxHealth);

        transform.localScale = Vector3.one * (1f + loopCount * 0.05f);
    }
}
