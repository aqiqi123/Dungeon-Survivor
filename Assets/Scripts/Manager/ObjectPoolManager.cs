using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance {  get; private set; }

    //Prefab->闲置对象队列(clone)
    private Dictionary<GameObject,Queue<GameObject>> poolDictionary=new Dictionary<GameObject,Queue<GameObject>>();

    //把生成的对象分类放在父节点下
    private Dictionary<GameObject,Transform>poolParents=new Dictionary<GameObject,Transform>();

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 从池中获取对象
    /// </summary>
    /// <param name="prefab">你需要生成的预制体</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    /// <returns>生成的实例</returns>
    public GameObject Spawn(GameObject prefab,Vector2 position,Quaternion rotation) {
        if(prefab == null) return null;

        //如果字典还没有这个Prefab的记录，就创建一个新的队列
        if (!poolDictionary.ContainsKey(prefab)) {
            poolDictionary.Add(prefab, new Queue<GameObject>());

            //创建一个父节点来整理层级面板
            GameObject parentObj=new GameObject(prefab.name+" Pool");
            parentObj.transform.SetParent(this.transform);
            poolParents.Add(prefab,parentObj.transform);
        }

        GameObject objToSpawn;

        //有闲置的就取出，没有就新建
        if (poolDictionary[prefab].Count > 0) {
            objToSpawn=poolDictionary[prefab].Dequeue();
        } else {
            objToSpawn=Instantiate(prefab);
            objToSpawn.transform.SetParent(poolParents[prefab]);
        }

        //初始化状态
        objToSpawn.transform.position=position;
        objToSpawn.transform.rotation=rotation;
        objToSpawn.SetActive(true);

        //尝试调用接口
        IPoolable poolable=objToSpawn.GetComponent<IPoolable>();
        if(poolable != null) {
            poolable.SetPrefabReference(prefab);

            poolable.OnSpawn();
        }

        return objToSpawn;
    }

    /// <summary>
    /// 将对象归还给池子
    /// </summary>
    /// <param name="obj">要归还的实例</param>
    /// <param name="prefab">它原本属于哪个Prefab</param>
    public void ReturnToPool(GameObject obj,GameObject prefab) {
        if (obj == null || prefab == null) return;

        //调用接口方法
        IPoolable poolable=obj.GetComponent<IPoolable>();
        if (poolable != null) {
            poolable.OnDespawn();
        }

        obj.SetActive(false);

        //放回队列
        if (poolDictionary.ContainsKey(prefab)) {
            poolDictionary[prefab].Enqueue(obj);
        } else {
            Destroy(obj);//如果归还时字典没了就销毁
        }
    }

    /// <summary>
    /// 清理所有对象池缓存，在返回主菜单或场景切换时调用
    /// </summary>
    public void ClearAllPools() {
        foreach (var pool in poolDictionary.Values) {
            while (pool.Count > 0) {
                GameObject obj = pool.Dequeue();
                if (obj != null) {
                    Destroy(obj);
                }
            }
        }

        foreach (var parent in poolParents.Values) {
            if (parent != null) {
                Destroy(parent.gameObject);
            }
        }

        poolDictionary.Clear();
        poolParents.Clear();
    }

    private void OnDestroy() {
        ClearAllPools();
    }
}
