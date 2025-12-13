using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlackHoleProjectile : ProjectileBase
{
    // 记录每个敌人下一次能受到伤害的时间点 <敌人ID, 下次受伤时间>
    private Dictionary<int, float> enemyHitTimers = new Dictionary<int, float>();

    protected override void Move() {
        if (PlayerStats.Instance == null) return;

        transform.position=PlayerStats.Instance.transform.position;
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        HandleAreaDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        HandleAreaDamage(collision);
    }

    private void HandleAreaDamage(Collider2D collision) {
        if (collision.TryGetComponent<IDamageable>(out var target)) {
            int enemyID = collision.gameObject.GetInstanceID();

            // 检查该敌人是否在冷却列表中
            if (enemyHitTimers.ContainsKey(enemyID)) {
                // 如果当前时间 < 下次受伤时间，说明还在冷却，跳过
                if (Time.time < enemyHitTimers[enemyID]) {
                    return;
                }
            }

            target.TakeDamage(damage);

            // 更新该敌人的冷却时间 = 当前时间 + 攻击间隔
            enemyHitTimers[enemyID] = Time.time + attackInterval;
        }
    }

    public override void OnDespawn() {
        base.OnDespawn();
        enemyHitTimers.Clear();
    }
}
