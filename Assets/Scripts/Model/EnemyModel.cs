using System.Collections;
using UnityEngine;

public class EnemyModel
{
    public EnemySO EnemyData { get; private set; }

    public float CurrentMaxHealth { get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentDamage { get; private set; }
    public float CurrentSizeMultiplier { get; private set; } = 1f;

    public void Initialize(EnemySO enemyData) {
        EnemyData = enemyData;

        CurrentMaxHealth = enemyData.MaxHealth;
        CurrentMoveSpeed = enemyData.MoveSpeed;
        CurrentDamage = enemyData.Damage;
        CurrentSizeMultiplier = 1f;
    }

    public bool ApplyBuffs(int loopCount, float healthGrowth, float healthLimit,
                           float speedGrowth, float speedLimit,
                           float damageGrowth, float damageLimit,
                           float sizeGrowth, float sizeLimit) {
        if (loopCount <= 0) return false;

        float hpMult = 1f + (loopCount * healthGrowth);
        float speedMult = 1f + (loopCount * speedGrowth);
        float dmgMult = 1f + (loopCount * damageGrowth);
        float sizeMult = 1f + (loopCount * sizeGrowth);

        hpMult = Mathf.Min(hpMult, healthLimit);
        speedMult = Mathf.Min(speedMult, speedLimit);
        dmgMult = Mathf.Min(dmgMult, damageLimit);
        sizeMult = Mathf.Min(sizeMult, sizeLimit);

        CurrentMaxHealth *= hpMult;
        CurrentMoveSpeed *= speedMult;
        CurrentDamage *= dmgMult;
        CurrentSizeMultiplier = sizeMult;

        return true;
    }
}
