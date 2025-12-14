using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesWeapon : WeaponBase {
    protected override void Attack() {
        StartCoroutine(SpawnSpikesRoutine(CurrentCount));
    }

    private IEnumerator SpawnSpikesRoutine(int count) {
        for (int i = 0; i < count; i++) {
            // 每次生成时重新获取最近的敌人
            Transform target = GetNearestEnemy();

            // 防止协程等待期间敌人已经被其他武器打死销毁了
            if (target != null) {
                SpawnSpikesAt(target.position);
            }

            if (i < count - 1) {
                yield return new WaitForSeconds(CurrentProjectileInterval);
            }
        }
    }

    private void SpawnSpikesAt(Vector3 targetPos) {
        if (CurrentBulletPrefab != null) {
            //在敌人位置生成
            GameObject lightningObj = ObjectPoolManager.Instance.Spawn(CurrentBulletPrefab, targetPos, Quaternion.identity);

            if (lightningObj.TryGetComponent<ProjectileBase>(out var projectile)) {
                projectile.Initialize(Vector2.zero, 0, CurrentDamage, CurrentDuration, CurrentAttackInterval, CurrentPierce);
                projectile.UpdateScale(CurrentArea);
            }
        }
    }
}
