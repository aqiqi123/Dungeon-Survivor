using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpreadShootAttack : EnemyAttackBase {
    [Header("射击设置")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 2.5f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float bulletLifetime = 4f;
    [SerializeField] private float detectRange = 10f;

    [Header("扇形发射设置")]
    [SerializeField] private int bulletCount = 3;
    [Tooltip("扇形的总角度，例如30度表示左右各偏15度")]
    [SerializeField] private float spreadAngle = 30f;

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

        Vector2 baseDirection = (playerTransform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletCount; i++) {
            float currentAngle;

            if (bulletCount == 1) {
                currentAngle = baseAngle;
            } else {
                float angleStep = spreadAngle / (bulletCount - 1);
                float offsetAngle = -spreadAngle / 2f + (i * angleStep);
                currentAngle = baseAngle + offsetAngle;
            }

            float radians = currentAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            GameObject bullet = ObjectPoolManager.Instance.Spawn(bulletPrefab, transform.position, Quaternion.identity);

            if (bullet.TryGetComponent<ProjectileBase>(out var projectile)) {
                projectile.Initialize(direction, bulletSpeed, enemyStats.CurrentDamage, bulletLifetime, 0f, 1);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (playerTransform != null) {
            Vector2 baseDirection = (playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < bulletCount; i++) {
                float currentAngle;
                if (bulletCount == 1) {
                    currentAngle = baseAngle;
                } else {
                    float angleStep = spreadAngle / (bulletCount - 1);
                    float offsetAngle = -spreadAngle / 2f + (i * angleStep);
                    currentAngle = baseAngle + offsetAngle;
                }

                float radians = currentAngle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * 2f);
            }
        }
    }
}
