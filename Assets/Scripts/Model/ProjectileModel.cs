using UnityEngine;

public class ProjectileModel
{
    public Vector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float Duration { get; private set; }
    public float AttackInterval { get; private set; }
    public int PierceCount { get; private set; }
    public float AreaMultiplier { get; private set; } = 1f;

    public DamageModel Damage { get; private set; }

    public void Initialize(Vector2 direction, float speed, float damage, float duration, float attackInterval, int pierceCount) {
        Direction = direction;
        Speed = speed;
        Duration = duration;
        AttackInterval = attackInterval;
        PierceCount = pierceCount;
        AreaMultiplier = 1f;

        Damage = new DamageModel();
        Damage.Initialize(damage);
    }

    public void UpdateAreaMultiplier(float areaMultiplier) {
        AreaMultiplier = areaMultiplier;
    }

    public bool ConsumePierce() {
        PierceCount--;
        return PierceCount > 0;
    }
}
