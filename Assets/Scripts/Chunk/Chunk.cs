using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : PoolableObject {
    public override void OnSpawn() {
        //随机旋转地面，减少重复
        int rot = Random.Range(0, 4) * 90;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        SpawnDecorations();
    }
    
    private void SpawnDecorations() {
        //以后可以在此编写生成障碍物、宝箱等的逻辑
    }

    public override void OnDespawn() {
        //重置旋转
        transform.rotation = Quaternion.identity;
    }
}
