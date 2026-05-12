using System.Collections.Generic;
using UnityEngine;


public abstract class PickupBase : PoolableObject {
    //记录当前场景中所有激活的拾取物(全局注册表）
    public static readonly List<PickupBase> ActivePickups = new List<PickupBase>();

    [Header("运动设置")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float magnetDistance; // 触发吸附的距离
    private float originalMoveSpeed;

    protected bool isMagnetized = false;
    protected Transform targetPlayer;

    public override void OnSpawn() {
        // 注册到激活列表
        ActivePickups.Add(this);

        originalMoveSpeed = moveSpeed;

        isMagnetized = false;

        if (PlayerStats.Instance != null) {
            targetPlayer = PlayerStats.Instance.transform;
        }
    }

    public override void OnDespawn() {
        //从激活列表中移除
        ActivePickups.Remove(this);
    }

    //提供给道具磁铁的方法
    public void ForceMagnetize() {
        isMagnetized = true;
    }

    protected virtual void Update() {
        if (targetPlayer == null) return;

        Vector3 playerPosition = targetPlayer.position;

        if (!isMagnetized) {
            float distance = Vector3.Distance(transform.position, playerPosition);

            // 获取玩家当前的拾取范围加成
            float pickupRange = magnetDistance * (PlayerStats.Instance != null ? PlayerStats.Instance.CurrentMagnet : 1f);

            if (distance < pickupRange) {
                isMagnetized = true;
            }
        }

        //飞向玩家
        if (isMagnetized) {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, moveSpeed * Time.deltaTime);
            moveSpeed += 20f * Time.deltaTime;

            if ((transform.position - playerPosition).sqrMagnitude < 0.25f)
            { 
                TriggerPickup();
            }
        }
    }

    private void TriggerPickup() {
        // 尝试获取当前脚本是否实现了接口
        if (this is IPickupable pickupItem) {
            // 执行具体的拾取效果
            if (PlayerStats.Instance != null) {
                pickupItem.OnPickUp();
            }
        }

        ReturnToPool();
    }
}