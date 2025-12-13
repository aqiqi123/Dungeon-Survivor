using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    private WeaponSO weaponData;

    public WeaponSO WeaponData=>weaponData;

    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float detectRange;

    private int currentLevelIndex = 0;

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

    // 对外公开的初始化方法
    public void Initialize(WeaponSO data) {
        weaponData = data;
        currentLevelIndex = 0; // 初始为 1级
        RecalculateStats();
    }

    // 对外公开的升级方法
    public void LevelUp(int newLevel) {
        // 防止数组越界
        if (weaponData != null && newLevel <= weaponData.LevelData.Count) {
            currentLevelIndex = newLevel - 1; // 转换为索引
            RecalculateStats();
        }
    }

    protected virtual void Start() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged += RecalculateStats;
        }
    }

    protected virtual void OnDestroy() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnPlayerStatsChanged -= RecalculateStats;
        }
    }

    public void RecalculateStats() {
        if (weaponData == null || PlayerStats.Instance == null) return;

        if (weaponData.LevelData == null || weaponData.LevelData.Count == 0) {
            Debug.LogError($"WeaponSO {weaponData.name} 缺少等级数据！");
            return;
        }

        if (currentLevelIndex >= weaponData.LevelData.Count) {
            currentLevelIndex = weaponData.LevelData.Count - 1;
        }

        //提取当前等级的结构体数据
        WeaponStats stats = weaponData.LevelData[currentLevelIndex];


        CurrentBulletPrefab = weaponData.BulletPrefab;

        CurrentDamage = stats.damage * PlayerStats.Instance.CurrentMight;

        CurrentCooldown = stats.cooldown * (1f - PlayerStats.Instance.CurrentCooldownReduction);

        CurrentCount = stats.count + (int)PlayerStats.Instance.CurrentAdditionalProjectileCount;

        CurrentArea = stats.area * PlayerStats.Instance.CurrentAreaMultiplier;

        CurrentSpeed = stats.speed * PlayerStats.Instance.CurrentProjectileSpeed;

        CurrentDuration = stats.duration * PlayerStats.Instance.CurrentDurationMultiplier;

        CurrentPierce = stats.pierce + PlayerStats.Instance.CurrentAdditionalPierceCount;

        CurrentProjectileInterval = stats.projectileInterval;

        CurrentAttackInterval = stats.attackInterval;

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

    // 当武器属性更新时调用，子类可重写此方法来更新场上已存在的子弹
    protected virtual void OnStatsUpdated() {
        // 默认实现为空，由子类根据需要重写
    }

    protected abstract void Attack();
}
