using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "SOs/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    public float MaxHealth=>maxHealth;
    public float MoveSpeed=>moveSpeed;
    public float Damage=>damage;
}
