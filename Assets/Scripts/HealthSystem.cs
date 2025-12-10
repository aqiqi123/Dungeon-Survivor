using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable {
    public float maxHealth {  get; private set; }
    public float currentHealth {  get; private set; }

    public UnityEvent OnDeath;
    public UnityEvent<float> OnTakeDamage;

    public void Initialize(float healthValue) {
        maxHealth=healthValue;
        currentHealth=maxHealth;
    }

    public void IncreaseMaxHealth(float amount) {
        maxHealth+=amount;
        currentHealth += amount;
    }

    public void Heal(float amount) {
        currentHealth+=amount;
        if(currentHealth > maxHealth)currentHealth=maxHealth;
    }

    private void OnEnable() // 配合对象池，每次激活时重置血量
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;

        // 触发受伤事件（可以绑定播放音效、闪白特效、飘字）
        OnTakeDamage?.Invoke(amount);

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDeath?.Invoke();
        //玩家可以触发游戏结束或者复活
        //敌人就回收对象
    }

    // 获取当前血量百分比（方便做UI血条）
    public float GetHealthPercentage() {
        return currentHealth / maxHealth;
    }
}
