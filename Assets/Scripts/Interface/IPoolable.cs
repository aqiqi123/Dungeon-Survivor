using UnityEngine;

public interface IPoolable
{
    //当从池中取出时调用，记得替代Start/Awake
    void OnSpawn();
    void OnDespawn();
}
