using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowWeapon : WeaponBase {
    protected override void Attack() {
        StartCoroutine(FireBurst());
    }

    private IEnumerator FireBurst() {
        if (PlayerStats.Instance == null) yield break;

        for (int i = 0; i < CurrentCount; i++) {
            Transform target = GetNearestEnemy();
            
            if (target != null) {
                Vector2 dir=(target.position - transform.position).normalized;

                if (CurrentBulletPrefab != null) {
                    GameObject bulletObj = ObjectPoolManager.Instance.Spawn(CurrentBulletPrefab, transform.position, Quaternion.identity);

                    if (bulletObj.TryGetComponent<ProjectileBase>(out var projectile)) {
                        projectile.Initialize(dir, CurrentSpeed, CurrentDamage, CurrentDuration,CurrentAttackInterval, CurrentPierce);
                        projectile.UpdateScale(CurrentArea);
                    }
                }
            }

            if (i < CurrentCount - 1) {
                yield return new WaitForSeconds(CurrentProjectileInterval);
            }
        }
    }
}
