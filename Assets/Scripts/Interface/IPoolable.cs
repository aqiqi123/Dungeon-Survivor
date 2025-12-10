using UnityEngine;

//接口决定要干什么
public interface IPoolable
{
    //当从池中取出时调用，记得替代Start/Awake
    void OnSpawn();
    void OnDespawn();

    void SetPrefabReference(GameObject prefab);
}
