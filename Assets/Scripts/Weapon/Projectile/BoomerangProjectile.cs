using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : ProjectileBase
{
    private float currentSpeed;

    public override void Initialize(Vector2 direction, float speed, float damage, float duration,float attackInterval, int pierceCount) {
        base.Initialize(direction, speed, damage, duration,attackInterval, 999);
        currentSpeed = speed;
    }

    protected override void Move() {
        // 核心逻辑：速度随时间递减，直到变成负数（反向飞）
        // 减速度 = 初始速度 * 2 / 持续时间的一半 (这只是个简单的物理模拟)
        float deceleration = speed * 1.5f * Time.deltaTime;
        currentSpeed -= deceleration;

        // 沿Y轴（前方）移动，如果速度是负的，它就会倒着飞
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
    }
}
