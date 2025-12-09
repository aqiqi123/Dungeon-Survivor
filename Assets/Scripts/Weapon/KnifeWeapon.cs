using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeWeapon : WeaponBase {
    protected override void Attack() {
        StartCoroutine(FireBurst());
    }

    private IEnumerator FireBurst() {
        if (PlayerStats.Instance == null) yield break;

        for (int i = 0; i < CurrentCount; i++) {
            Vector2 bulletDir = PlayerStats.Instance.GetFacingDirection();

            if (CurrentBulletPrefab != null) {
                GameObject bulletObj = ObjectPoolManager.Instance.Spawn(CurrentBulletPrefab, transform.position, Quaternion.identity);

                if (bulletObj.TryGetComponent<ProjectileBase>(out var projectile)) {
                    projectile.Initialize(bulletDir, CurrentSpeed, CurrentDamage, CurrentDuration,CurrentAttackInterval, CurrentPierce);
                }
            }

            if(i<CurrentCount - 1) {
                yield return new WaitForSeconds(CurrentProjectileInterval);
            }
        }
    }
}
