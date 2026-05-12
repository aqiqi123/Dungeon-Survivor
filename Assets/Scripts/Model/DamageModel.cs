using UnityEngine;

public class DamageModel
{
    public float BaseDamage { get; private set; }
    public float Multiplier { get; private set; } = 1f;
    public float FlatBonus { get; private set; }

    public float FinalDamage => Mathf.Max(0f, (BaseDamage + FlatBonus) * Multiplier);

    public void Initialize(float baseDamage) {
        BaseDamage = baseDamage;
        Multiplier = 1f;
        FlatBonus = 0f;
    }

    public void SetBaseDamage(float baseDamage) {
        BaseDamage = baseDamage;
    }

    public void ApplyMultiplier(float multiplier) {
        Multiplier *= multiplier;
    }

    public void AddFlatBonus(float bonus) {
        FlatBonus += bonus;
    }
}
