using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : ProjectileBase
{
    [Tooltip("闪电落雷的伤害半径（容错范围）")]
    [SerializeField] private float hitRadius;

    [Tooltip("伤害延迟时间（配合动画劈下来的那一帧）")]
    [SerializeField] private float damageDelay;

    public override void Initialize(Vector2 direction, float speed, float damage, float duration,float attackInterval, int pierceCount) {
        base.Initialize(direction, 0, damage, duration,attackInterval, pierceCount);

        transform.rotation = Quaternion.identity;

        StartCoroutine(StrikeProcess());
    }

    protected override void Move() {
        //定点打击不需要移动
    }

    private IEnumerator StrikeProcess() {
        yield return new WaitForSeconds(damageDelay);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);

        foreach (var hit in hits) {
            if (hit.TryGetComponent<IDamageable>(out var target)) {
                target.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
