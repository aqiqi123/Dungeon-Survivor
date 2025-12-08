using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visual;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector2 moveDir = GameInput.instance.GetMovementVectorNormalized();

        if (moveDir.x != 0) {
            visual.flipX = moveDir.x < 0;
        }

        rb.velocity = moveDir * PlayerStats.Instance.CurrentMoveSpeed;
    }
}
