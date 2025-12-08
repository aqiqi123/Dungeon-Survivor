using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("ÅäÖÃ")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval;
    private float timer;

    private void Update() {
        timer += Time.deltaTime;

        if(timer > spawnInterval) {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy() {
        if (enemyPrefab == null) return;
        if (ObjectPoolManager.Instance == null) return;

        GameObject enemyObj=ObjectPoolManager.Instance.Spawn(enemyPrefab,transform.position,Quaternion.identity);
    }
}
