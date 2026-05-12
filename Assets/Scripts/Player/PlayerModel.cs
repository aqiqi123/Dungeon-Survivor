using System.Collections;
using UnityEngine;

public class PlayerModel
{
    public CharacterSO CharacterData { get; private set; }

    public float CurrentMaxHealth { get; private set; }
    public float CurrentMoveSpeed { get; private set; }
    public float CurrentMight { get; private set; }
    public float CurrentMagnet { get; private set; }
    public float CurrentCooldownReduction { get; private set; }
    public int CurrentAdditionalProjectileCount { get; private set; }
    public float CurrentProjectileSpeed { get; private set; }
    public float CurrentDurationMultiplier { get; private set; }
    public float CurrentAreaMultiplier { get; private set; }
    public int CurrentAdditionalPierceCount { get; private set; }
    public int CurrentGold { get; private set; }
    public float CurrentLuck { get; private set; }

    public Vector2 FacingDirection { get; private set; }

    public void Initialize(CharacterSO characterData) {
        CharacterData = characterData;

        CurrentMaxHealth = characterData.MaxHealth;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentMagnet = characterData.Magnet;
        CurrentCooldownReduction = characterData.CooldownReduction;
        CurrentAdditionalProjectileCount = characterData.AdditionalProjectileCount;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentDurationMultiplier = characterData.DurationMultiplier;
        CurrentAreaMultiplier = characterData.AreaMultiplier;
        CurrentAdditionalPierceCount = characterData.AdditionalPierceCount;
        CurrentLuck = characterData.Luck;
        CurrentGold = 0;

        FacingDirection = Vector2.right;
    }

    public bool IncreaseMight(float amount) {
        if (amount == 0) return false;
        CurrentMight += amount;
        return true;
    }

    public bool IncreaseMagnet(float percentage) {
        if (percentage == 0) return false;
        CurrentMagnet *= (1 + percentage);
        return true;
    }

    public bool IncreaseCooldownReduction(float amount) {
        if (amount == 0) return false;
        CurrentCooldownReduction += amount;
        CurrentCooldownReduction = Mathf.Clamp(CurrentCooldownReduction, 0f, 0.9f);
        return true;
    }

    public bool IncreaseProjectileCount(int amount) {
        if (amount == 0) return false;
        CurrentAdditionalProjectileCount += amount;
        return true;
    }

    public bool IncreaseMaxHealth(float amount) {
        if (amount == 0) return false;
        CurrentMaxHealth += amount;
        return true;
    }

    public bool IncreaseArea(float percentage) {
        if (percentage == 0) return false;
        CurrentAreaMultiplier *= (1 + percentage);
        return true;
    }

    public bool IncreaseSpeed(float percentage) {
        if (percentage == 0) return false;
        CurrentProjectileSpeed += percentage;
        return true;
    }

    public bool IncreaseDuration(float percentage) {
        if (percentage == 0) return false;
        CurrentDurationMultiplier += percentage;
        return true;
    }

    public bool IncreaseMoveSpeed(float percentage) {
        if (percentage == 0) return false;
        CurrentMoveSpeed *= (1 + percentage);
        return true;
    }

    public bool IncreasePierceCount(int amount) {
        if (amount == 0) return false;
        CurrentAdditionalPierceCount += amount;
        return true;
    }

    public bool IncreaseLuck(float amount) {
        if (amount == 0) return false;
        CurrentLuck += amount;
        return true;
    }

    public void AddGold(int amount) {
        CurrentGold += amount;
    }

    public void UpdateFacingDirection(Vector2 moveDir) {
        if (moveDir != Vector2.zero) {
            FacingDirection = moveDir;
        }
    }
}
