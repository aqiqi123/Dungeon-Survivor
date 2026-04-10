using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats),typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;

    private EnemyStats enemyStats;
    private Rigidbody2D rb;

    // 给空间管理器读取的“追玩家目标速度”
    public Vector2 DesiredVelocity { get; private set; }
    public EnemyStats Stats => enemyStats;
    public Rigidbody2D Rb => rb;
    public Vector2 Position => rb.position;

    private void Awake() {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EnemyMovementBatchCalculator.Instance?.Register(this);
    }

    private void OnDisable()
    {
        EnemyMovementBatchCalculator.Instance?.UnRegister(this);
    }

    public void SetDesiredVelocity(Vector2 velocity)
    {
        DesiredVelocity = velocity;
        if (visual != null && velocity.x != 0f)
        {
            visual.flipX = velocity.x < 0f;
        }
    }
}
