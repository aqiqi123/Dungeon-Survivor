using UnityEngine;


public abstract class PickupBase : PoolableObject {
    [Header("运动设置")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float magnetDistance; // 触发吸附的距离
    private float originalMoveSpeed;

    protected bool isMagnetized = false;
    protected Transform targetPlayer;

    public override void OnSpawn() {

        originalMoveSpeed = moveSpeed;

        isMagnetized = false;

        if (PlayerStats.Instance != null) {
            targetPlayer = PlayerStats.Instance.transform;
        }
    }

    public override void OnDespawn() {
        
    }

    protected virtual void Update() {
        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        // 获取玩家当前的拾取范围加成
        float pickupRange = magnetDistance * (PlayerStats.Instance != null ? PlayerStats.Instance.CurrentMagnet : 1f);

        if (!isMagnetized && distance < pickupRange) {
            isMagnetized = true;
        }

        //飞向玩家
        if (isMagnetized) {
            // 越飞越快的效果
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, moveSpeed * Time.deltaTime);
            moveSpeed += 10f * Time.deltaTime;

            //判定拾取成功
            if (distance < 0.5f) {
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