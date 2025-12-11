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
}
