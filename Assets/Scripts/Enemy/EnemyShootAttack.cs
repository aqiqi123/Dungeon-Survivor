using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootAttack : EnemyAttackBase
{
    [Header("…‰ª˜…Ë÷√")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float bulletLifetime = 4f;
    [SerializeField] private float detectRange = 10f;

    private Transform playerTransform;
    private float shootTimer;

    protected override void Awake() {
        base.Awake();
        shootTimer = shootInterval;
    }

    private void Update() {
        if (playerTransform == null) {
            if (PlayerStats.Instance != null) {
                playerTransform = PlayerStats.Instance.transform;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectRange) {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f) {
                Shoot();
                shootTimer = shootInterval;
            }
        }
    }

    private void Shoot() {
        if (bulletPrefab == null || playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        GameObject bullet = ObjectPoolManager.Instance.Spawn(bulletPrefab, transform.position, Quaternion.identity);

        if (bullet.TryGetComponent<ProjectileBase>(out var enemyBullet)) {
            enemyBullet.Initialize(direction, bulletSpeed, enemyStats.CurrentDamage, bulletLifetime,0f,1);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
