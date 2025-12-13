using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : PoolableObject {
    protected Vector2 direction;
    protected float speed;
    protected float damage;
    protected float duration;
    protected float attackInterval;
    protected int pierceCount;

    public virtual void Initialize(Vector2 direction,float speed,float damage,float duration,float attackInterval,int pierceCount) {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.attackInterval = attackInterval;
        this.pierceCount = pierceCount;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward);

        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), duration);
    }

    protected virtual void Update() {
        Move();
    }

    //让武器修改投射物的大小
    public virtual void UpdateScale(float areaMultiplier) {
        transform.localScale = Vector3.one * areaMultiplier;
    }

    protected virtual void Move() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent<IDamageable>(out var target)) {
            OnHit(target);
        }
    }

    protected virtual void OnHit(IDamageable target) {
        target.TakeDamage(damage);

        pierceCount--;
        if (pierceCount <= 0) {
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
