using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("生成范围")]
    [SerializeField] private Vector2 range;

    [Header("全局限制")]
    [SerializeField] private int maxGlobalEnemies = 300;
    [SerializeField] private float minSpawnInterval = 0.2f;
    [SerializeField] private float spawnIntervalReductionFactor = 0.9f;//每波结束后生成间隔的缩减系数

    [System.Serializable]
    public class Wave {
        public GameObject enemyPrefab;//生成哪个敌人
        public float spawnInterval;//多少秒生成一次
        [HideInInspector] public float spawnTimer;//计时器
        public int enemiesPerWave;//这波生成多少个该敌人
        public int spawnedEnemyCount;//已经生成的敌人
    }

    [Header("波次设置")]
    public List<Wave> waves;
    public int waveNumber;//当前处于哪波

    [Header("难度成长设置 (每循环一轮增加多少)")]
    private int loopCount = 0;

    [Tooltip("生命值成长率 (0.1 代表每轮增加 10%)")]
    [SerializeField] private float healthGrowth = 0.2f;
    [Tooltip("生命值最大倍率上限 (3 代表最多是基础血量的3倍)")]
    [SerializeField] private float healthLimit = 5.0f;

    [Tooltip("移速成长率")]
    [SerializeField] private float speedGrowth = 0.05f;
    [Tooltip("移速倍率上限")]
    [SerializeField] private float speedLimit = 1.5f;

    [Tooltip("伤害成长率")]
    [SerializeField] private float damageGrowth = 0.1f;
    [Tooltip("伤害倍率上限")]
    [SerializeField] private float damageLimit = 3.0f;

    [Tooltip("体型成长率 (0.1 代表每轮增加 10%)")]
    [SerializeField] private float sizeGrowth = 0.1f;
    [Tooltip("体型最大倍率上限 (2 代表最多是原体型的2倍)")]
    [SerializeField] private float sizeLimit = 2.0f;

    private Transform playerTransfrom;
    private PlayerStats playerStatsCache;
    private int currentActiveEnemies = 0;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (playerStatsCache == null) {
            playerStatsCache = PlayerStats.Instance;
            if (playerStatsCache == null) return;
        }

        if (playerTransfrom == null) {
            playerTransfrom = playerStatsCache.transform;
        }

        Wave currentWave = waves[waveNumber];

        currentWave.spawnTimer += Time.deltaTime;

        if(currentWave.spawnTimer > currentWave.spawnInterval) {
            currentWave.spawnTimer = 0f;
            SpawnEnemy(currentWave);
        }

        if(currentWave.spawnedEnemyCount>= currentWave.enemiesPerWave) {
            currentWave.spawnedEnemyCount = 0;

            if (currentWave.spawnInterval >minSpawnInterval) {
                currentWave.spawnInterval *= spawnIntervalReductionFactor;
            }

            waveNumber++;

            if (waveNumber >= waves.Count) {
                waveNumber = 0;
                loopCount++;
            }
        }
    }

    private void SpawnEnemy(Wave wave) {
        if (currentActiveEnemies >= maxGlobalEnemies) {
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPosition(playerTransfrom.position);

        GameObject enemy = ObjectPoolManager.Instance.Spawn(wave.enemyPrefab, spawnPos, Quaternion.identity);

        if (enemy.TryGetComponent<EnemyStats>(out var stats)) {
            stats.ApplyBuffs(loopCount, healthGrowth, healthLimit, speedGrowth, speedLimit, damageGrowth, damageLimit, sizeGrowth, sizeLimit);
        }

        currentActiveEnemies++;
        wave.spawnedEnemyCount++;
    }

    // 矩形边缘生成算法
    private Vector2 GetRandomSpawnPosition(Vector3 centerPos) {
        // 随机决定是 "左右边" 还是 "上下边" (50%概率)
        bool isVertical = Random.value > 0.5f;

        // 随机决定是 "正方向" 还是 "负方向" (比如 上vs下，或 右vs左)
        int sign = (Random.value > 0.5f) ? 1 : -1;

        float x, y;

        if (isVertical) {
            // 固定 Y 轴 (上或下)，随机 X 轴
            x = Random.Range(-range.x, range.x);
            y = range.y * sign;
        } else {
            // 固定 X 轴 (左或右)，随机 Y 轴
            x = range.x * sign;
            y = Random.Range(-range.y, range.y);
        }

        return (Vector2)centerPos + new Vector2(x, y);
    }

    // 敌人被销毁时调用
    public void OnEnemyDespawned() {
        currentActiveEnemies--;
        if (currentActiveEnemies < 0) currentActiveEnemies = 0;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;

        Vector3 center = Application.isPlaying && playerStatsCache != null ?
                         playerStatsCache.transform.position : transform.position;

        Gizmos.DrawWireCube(center, range * 2);
    }
}
