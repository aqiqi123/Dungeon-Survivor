using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    private WeaponModel model;

    public WeaponSO WeaponData => model != null ? model.WeaponData : null;

    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float detectRange;

    public GameObject CurrentBulletPrefab => model != null ? model.CurrentBulletPrefab : null;
    public float CurrentDamage => model != null ? model.CurrentDamage : 0f;
    public float CurrentCooldown => model != null ? model.CurrentCooldown : 0f;
    public float CurrentSpeed => model != null ? model.CurrentSpeed : 0f;
    public float CurrentDuration => model != null ? model.CurrentDuration : 0f;
    public int CurrentCount => model != null ? model.CurrentCount : 0;
    public float CurrentArea => model != null ? model.CurrentArea : 0f;
    public int CurrentPierce => model != null ? model.CurrentPierce : 0;
    public float CurrentProjectileInterval => model != null ? model.CurrentProjectileInterval : 0f;
    public float CurrentAttackInterval => model != null ? model.CurrentAttackInterval : 0f;

    protected float timer;

    // 对外公开的初始化方法
    public void Initialize(WeaponSO data) {
        model = new WeaponModel();
        model.Initialize(data);
        RecalculateStats();
    }

    // 对外公开的升级方法
    public void LevelUp(int newLevel) {
        if (model == null) return;
        model.LevelUp(newLevel);
        RecalculateStats();
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
        if (model == null || PlayerStats.Instance == null) return;
        if (!model.RecalculateStats(PlayerStats.Instance.Model)) return;

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
            float dst = (transform.position-hit.transform.position).sqrMagnitude;//计算距离的平方，避免开方运算

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
