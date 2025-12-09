using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWeapon : WeaponBase {
    protected override void Attack() {
        List<Transform> targets = GetRandomEnemy();

        int attackCount=Mathf.Min(CurrentCount, targets.Count);

        StartCoroutine(SpawnLightningRoutine(targets, attackCount));
    }

    private IEnumerator SpawnLightningRoutine(List<Transform> targets, int count) {
        for (int i = 0; i < count; i++) {
            Transform target = targets[i];

            // 防止协程等待期间敌人已经被其他武器打死销毁了
            if (target != null) {
                SpawnLightningAt(target.position);
            }

            yield return new WaitForSeconds(CurrentProjectileInterval);
        }
    }

    private void SpawnLightningAt(Vector3 targetPos) {
        if (CurrentBulletPrefab != null) {
            //在敌人位置生成
            GameObject lightningObj = ObjectPoolManager.Instance.Spawn(CurrentBulletPrefab, targetPos, Quaternion.identity);

            if (lightningObj.TryGetComponent<ProjectileBase>(out var projectile)) {
                projectile.Initialize(Vector2.zero, 0, CurrentDamage, CurrentDuration,CurrentAttackInterval,CurrentPierce);
            }
        }
    }
}
