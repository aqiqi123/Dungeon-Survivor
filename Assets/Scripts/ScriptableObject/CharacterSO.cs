using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterSO",menuName ="SOs/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    public float MoveSpeed=> moveSpeed;
}
