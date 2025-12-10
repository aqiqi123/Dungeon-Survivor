using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Animator animator;

    private readonly string MOVESPEED = "moveSpeed";

    private Rigidbody2D rb;

    public Vector2 facingDirection {  get; private set; }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        facingDirection=Vector2.right;
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 moveDir = GameInput.instance.GetMovementVectorNormalized();

        if (moveDir.x != 0) {
            visual.flipX = moveDir.x < 0;
        }

        if (moveDir != Vector2.zero) {
            facingDirection = moveDir;
        }

        rb.velocity = moveDir * PlayerStats.Instance.CurrentMoveSpeed;

        animator.SetFloat(MOVESPEED, moveDir.sqrMagnitude);
    }
}
