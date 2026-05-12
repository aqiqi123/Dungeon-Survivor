using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : ProjectileBase
{
    [SerializeField] private float explosionRadius=2f;
    [SerializeField] private LayerMask enemyLayer;

    [Tooltip("爆炸时的特效Prefab")]
    [SerializeField] private GameObject explosionVfxPrefab;

    private float currentAreaMultiplier = 1f;

    public override void UpdateScale(float areaMultiplier) {
        base.UpdateScale(areaMultiplier);
        currentAreaMultiplier = areaMultiplier;
    }

    protected override void OnHit(IDamageable target) {
        if (explosionVfxPrefab != null) {
            GameObject vfx = ObjectPoolManager.Instance.Spawn(explosionVfxPrefab, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * currentAreaMultiplier;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius * currentAreaMultiplier,enemyLayer);

        foreach (var hit in hits) {
            if (hit.TryGetComponent<IDamageable>(out var enemy)) {
                enemy.TakeDamage(damage);
            }
        }

        CancelInvoke(nameof(ReturnToPool));
        ReturnToPool();
        
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius * currentAreaMultiplier);
    }
}
