using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomLootItem {
    public GameObject prefab;
    [Range(0, 100)] public float dropChance;
}

[CreateAssetMenu(fileName ="LootTable",menuName ="SOs/Loot Table")]
public class LootTableSO : ScriptableObject
{
    [Header("必掉落物品")]
    public GameObject guaranteedDrop;

    [Header("随机掉落")]
    public List<RandomLootItem> possibleDrops;
}
