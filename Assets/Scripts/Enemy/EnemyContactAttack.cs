using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactAttack : EnemyAttackBase
{
    [SerializeField] private float damageInterval = 1f;
    private float timer;

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {

        if (timer > 0) return;

        if (other.gameObject.layer==playerLayer) {
            if (other.TryGetComponent<IDamageable>(out var target)) {
                target.TakeDamage(enemyStats.CurrentDamage);

                timer = damageInterval;
            }
        }
    }
}
