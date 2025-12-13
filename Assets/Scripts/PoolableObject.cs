using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//继承自接口的抽象基类决定了怎么做
public abstract class PoolableObject : MonoBehaviour, IPoolable {
    //让clone知道自己是哪个Prefab生成的
    private GameObject myPrefab;

    public void SetPrefabReference(GameObject prefab) {
        myPrefab = prefab;
    }

    public void ReturnToPool() {
        // 停止该对象上所有的 DOTween 动画，防止回池后继续执行
        transform.DOKill();
        
        if (myPrefab != null) {
            if (ObjectPoolManager.Instance != null) {
                ObjectPoolManager.Instance.ReturnToPool(this.gameObject, myPrefab);
            } else {
                Destroy(gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }

    public abstract void OnSpawn();
    public abstract void OnDespawn();
}
