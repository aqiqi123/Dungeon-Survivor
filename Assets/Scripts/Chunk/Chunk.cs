using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : PoolableObject {
    public override void OnSpawn() {

        SpawnDecorations();
    }
    
    private void SpawnDecorations() {
        //以后可以在此编写生成障碍物、宝箱等的逻辑
    }

    public override void OnDespawn() {

    }
}
