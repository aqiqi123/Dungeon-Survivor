using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成范围")]
    [SerializeField] private Vector2 range;

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

    private Transform playerTransfrom;

    private void Update() {
        if (PlayerStats.Instance == null) return;
        playerTransfrom=PlayerStats.Instance.transform;

        Wave currentWave = waves[waveNumber];

        currentWave.spawnTimer += Time.deltaTime;

        if(currentWave.spawnTimer > currentWave.spawnInterval) {
            currentWave.spawnTimer = 0f;
            SpawnEnemy(currentWave);
        }

        if(currentWave.spawnedEnemyCount>= currentWave.enemiesPerWave) {
            currentWave.spawnedEnemyCount = 0;

            if (currentWave.spawnInterval > 0.5f) {
                currentWave.spawnInterval *= 0.9f;
            }

            waveNumber = (waveNumber + 1) % waves.Count;
        }
    }

    private void SpawnEnemy(Wave wave) {
        Vector3 spawnPos = GetRandomSpawnPosition(PlayerStats.Instance.transform.position);

        GameObject enemy = ObjectPoolManager.Instance.Spawn(wave.enemyPrefab, spawnPos, Quaternion.identity);

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

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;

        Vector3 center = Application.isPlaying && PlayerStats.Instance != null ?
                         PlayerStats.Instance.transform.position : transform.position;

        Gizmos.DrawWireCube(center, range * 2);
    }
}
