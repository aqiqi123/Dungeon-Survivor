using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;

    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float detectRange;

    public GameObject CurrentBulletPrefab { get; private set; }
    public float CurrentDamage {  get; private set; }
    public float CurrentCooldown { get; private set; }  
    public float CurrentSpeed { get; private set; }
    public float CurrentDuration { get; private set; }
    public int CurrentCount { get; private set; }
    public float CurrentArea { get; private set; }
    public int CurrentPierce { get; private set; }
    public float CurrentProjectileInterval { get; private set; }
    public float CurrentAttackInterval { get; private set; }

    protected float timer;

    protected virtual void Start() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged += RecalculateStats;
        }

        RecalculateStats();
    }

    protected virtual void OnDestroy() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged -= RecalculateStats;
        }
    }

    public void RecalculateStats() {
        if (weaponData == null || PlayerStats.Instance == null) return;

        CurrentBulletPrefab = weaponData.BulletPrefab;

        // 伤害 = 基础伤害 * 玩家力量系数 (Might)
        CurrentDamage = weaponData.Damage * PlayerStats.Instance.CurrentMight;

        // 冷却 = 基础冷却 * (1 - 冷却缩减率)
        // 例如：1秒冷却，缩减0.1(10%) -> 1 * 0.9 = 0.9秒
        CurrentCooldown = weaponData.Cooldown * (1f - PlayerStats.Instance.CurrentCooldownReduction);

        // 数量 = 基础数量 + 玩家额外数量
        CurrentCount = weaponData.Count + (int)PlayerStats.Instance.CurrentAdditionalProjectileCount;

        // 范围 = 基础范围 * 范围加成
        CurrentArea = weaponData.Area * PlayerStats.Instance.CurrentAreaMultiplier;

        // 速度 = 基础速度 * 速度加成
        CurrentSpeed = weaponData.Speed * PlayerStats.Instance.CurrentProjectileSpeed;

        // 持续时间 = 基础时间 * 持续时间加成
        CurrentDuration = weaponData.Duration * PlayerStats.Instance.CurrentDurationMultiplier;

        //穿透=基础穿透+穿透加成
        CurrentPierce = weaponData.Pierce + PlayerStats.Instance.CurrentAdditionalPierceCount;

        //子弹间隔时间
        CurrentProjectileInterval = weaponData.ProjectileInterval;

        //持续性武器攻击间隔
        CurrentAttackInterval = weaponData.AttackInterval;

        // 如果是光环类武器，这里可能需要更新一下 transform.localScale 来立刻反映范围变化
        // 调用一个新方法，通知子类数值变了
        OnStatsUpdated();
    }

    protected virtual void Update() {
        timer-= Time.deltaTime;
        if (timer < 0) {
            Attack();
            timer=CurrentCooldown;
        }
    }

    //获取范围内最近的敌人
    protected virtual Transform GetNearestEnemy() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange, enemyLayer);

        Transform bestTarget = null;
        float minDst = Mathf.Infinity;

        foreach (var hit in hits) {
            float dst = Vector3.Distance(transform.position, hit.transform.position);

            if (dst < minDst) {
                minDst = dst;
                bestTarget = hit.transform;
            }
        }

        return bestTarget;
    }

    //获取范围内随机敌人
    protected virtual List<Transform> GetRandomEnemy() {
        Collider2D[] allEnemies = Physics2D.OverlapCircleAll(transform.position, detectRange, enemyLayer);

        //筛选出有效的、活着的敌人
        List<Transform> validTargets = new List<Transform>();
        foreach (var collider in allEnemies) {
            if (collider.gameObject.activeInHierarchy) {
                validTargets.Add(collider.transform);
            }
        }

        if (validTargets.Count > 0) {
            // 洗牌算法 (Fisher-Yates Shuffle)
            for (int i = 0; i < validTargets.Count; i++) {
                Transform temp = validTargets[i];
                int randomIndex = Random.Range(i, validTargets.Count);
                validTargets[i] = validTargets[randomIndex];
                validTargets[randomIndex] = temp;
            }
        }

        return validTargets;
    }

    //绘制武器攻击范围
    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    protected virtual void OnStatsUpdated() { }

    protected abstract void Attack();
}
