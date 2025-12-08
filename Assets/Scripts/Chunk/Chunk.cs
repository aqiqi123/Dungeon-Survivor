using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour, IPoolable {
    [SerializeField] private GameObject[] decorations;//树、石头等

    public void OnSpawn() {
        //随机旋转地面，减少重复
        int rot = Random.Range(0, 4) * 90;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        SpawnDecorations();
    }
    
    private void SpawnDecorations() {
        //以后可以在此编写生成障碍物、宝箱等的逻辑
    }

    public void OnDespawn() {
        //重置旋转
        transform.rotation = Quaternion.identity;
    }
}
