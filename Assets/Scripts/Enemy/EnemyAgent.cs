using UnityEngine;


/// <summary>
/// 负责给四叉树提供轻量的数据
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnemyAgent : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }
    public CircleCollider2D Col { get; private set; }
    public EnemyMovement Em { get; private set; }

    public Vector2 Position => Rb.position;
    public float Radius
    {
        get
        {
            float scale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            return Col.radius * scale;
        }
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Col = GetComponent<CircleCollider2D>();
        Em = GetComponent<EnemyMovement>();
    }
}
