using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : PoolableObject {
    protected ProjectileModel model;

    public virtual void Initialize(Vector2 direction,float speed,float damage,float duration,float attackInterval,int pierceCount) {
        model = new ProjectileModel();
        model.Initialize(direction, speed, damage, duration, attackInterval, pierceCount);

        float angle = Mathf.Atan2(model.Direction.y, model.Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward);

        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), model.Duration);
    }

    protected virtual void Update() {
        Move();
    }

    //让武器修改投射物的大小
    public virtual void UpdateScale(float areaMultiplier) {
        transform.localScale = Vector3.one * areaMultiplier;
        if (model != null) {
            model.UpdateAreaMultiplier(areaMultiplier);
        }
    }

    protected virtual void Move() {
        if (model == null) return;
        transform.Translate(Vector3.up * model.Speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent<IDamageable>(out var target)) {
            OnHit(target);
        }
    }

    protected virtual void OnHit(IDamageable target) {
        if (model == null) return;

        target.TakeDamage(model.Damage.FinalDamage);

        if (!model.ConsumePierce()) {
            CancelInvoke(nameof(ReturnToPool));
            ReturnToPool();
        }
    }

    public override void OnDespawn() {
        
    }

    public override void OnSpawn() {
        CancelInvoke(nameof(ReturnToPool));
    }
}
