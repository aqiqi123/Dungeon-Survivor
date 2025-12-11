using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : ProjectileBase
{
    [SerializeField] private float explosionRadius=2f;

    [Tooltip("爆炸时的特效Prefab")]
    [SerializeField] private GameObject explosionVfxPrefab;

    protected override void OnHit(IDamageable target) {
        if (explosionVfxPrefab != null) {
            GameObject vfx = ObjectPoolManager.Instance.Spawn(explosionVfxPrefab, transform.position, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

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
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
