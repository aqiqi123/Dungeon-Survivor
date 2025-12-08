using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats),typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;

    private EnemyStats enemyStats;
    private Rigidbody2D rb;

    private void Awake() {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (PlayerStats.Instance == null) {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 moveDir = (PlayerStats.Instance.transform.position-transform.position).normalized;

        if (moveDir.x != 0) {
            visual.flipX = moveDir.x < 0;
        }

        rb.velocity = moveDir * enemyStats.CurrentMoveSpeed;
    }
}
