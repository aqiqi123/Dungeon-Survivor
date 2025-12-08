using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterSO",menuName ="SOs/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;

    public float MaxHealth=>maxHealth;
    public float MoveSpeed=> moveSpeed;
}
