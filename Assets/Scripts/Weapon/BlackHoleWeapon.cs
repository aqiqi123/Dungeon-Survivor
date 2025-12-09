using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleWeapon : WeaponBase {
    private GameObject currentActiveBlackHole;

    protected override void Attack() {
        if (currentActiveBlackHole != null && currentActiveBlackHole.activeInHierarchy) {
            return;
        }

        StartCoroutine(SpawnBlackHole());
    }

    private IEnumerator SpawnBlackHole() {
        if (PlayerStats.Instance == null) yield break;

        if (CurrentBulletPrefab != null) {
            GameObject bulletObj = ObjectPoolManager.Instance.Spawn(CurrentBulletPrefab, transform.position, Quaternion.identity);
            currentActiveBlackHole = bulletObj;

            if (bulletObj.TryGetComponent<BlackHoleProjectile>(out var projectile)) {
                projectile.Initialize(Vector2.zero, 0, CurrentDamage, CurrentDuration, CurrentAttackInterval, 999);

                projectile.UpdateScale(CurrentArea);
            }
        }
    }

    // 当玩家获得增加范围的被动道具时，需要实时更新当前场上大蒜的大小
    protected override void OnStatsUpdated() {
        base.OnStatsUpdated();

        if (currentActiveBlackHole != null && currentActiveBlackHole.activeInHierarchy) {
            if (currentActiveBlackHole.TryGetComponent<BlackHoleProjectile>(out var projectile)) {
                projectile.UpdateScale(CurrentArea);
            }
        }
    }
}