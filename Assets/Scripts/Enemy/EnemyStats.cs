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

        // 重新监听死亡事件 (防止重复监听，先移除再添加)
        healthSystem.OnDeath.RemoveListener(HandleDeath);
        healthSystem.OnDeath.AddListener(HandleDeath);
    }

    public override void OnDespawn() {
        healthSystem.OnDeath.RemoveListener(HandleDeath);
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

    private void HandleDeath() {
        ReturnToPool();
    }
}
