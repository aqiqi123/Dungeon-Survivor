using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyLootSpawner : MonoBehaviour
{
    [SerializeField] private LootTableSO lootTable;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnEnable() {
        healthSystem.OnDeath.AddListener(OnEnemyDeath);
    }

    private void OnDisable() {
        healthSystem.OnDeath.RemoveListener(OnEnemyDeath);
    }

    private void OnEnemyDeath() {
        if (lootTable == null) return;

        if (lootTable.guaranteedDrop != null) {
            SpawnLoot(lootTable.guaranteedDrop, transform.position);
        }

        foreach (var item in lootTable.possibleDrops) {
            if (Random.Range(0f, 100f) <= item.dropChance) {
                Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                SpawnLoot(item.prefab, transform.position + offset);
            }
        }
    }

    private void SpawnLoot(GameObject prefab, Vector3 position) {
        if (prefab == null) return;

        GameObject lootObj = ObjectPoolManager.Instance.Spawn(prefab, position, Quaternion.identity);
    }
}
