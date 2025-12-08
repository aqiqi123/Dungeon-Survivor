using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "SOs/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private Sprite icon;
    [Space]
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    public string EnemyName=>enemyName;
    public Sprite Icon=>icon;
    public float MaxHealth=>maxHealth;
    public float MoveSpeed=>moveSpeed;
    public float Damage=>damage;
}
