using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable {
    public float maxHealth {  get; private set; }
    public float currentHealth {  get; private set; }

    public event Action OnDeath;
    public event Action<float> OnTakeDamage;

    public event Action<float, float> OnHealthChanged;

    public void Initialize(float healthValue) {
        maxHealth=healthValue;
        currentHealth=maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxHealth(float amount) {
        maxHealth+=amount;
        currentHealth += amount;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(float amount) {
        currentHealth+=amount;
        if(currentHealth > maxHealth)currentHealth=maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnEnable() // 配合对象池，每次激活时重置血量
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;

        // 触发受伤事件（可以绑定播放音效、闪白特效、飘字）
        OnTakeDamage?.Invoke(amount);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDeath?.Invoke();
    }
}
