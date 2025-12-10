using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public abstract class EnemyAttackBase : MonoBehaviour
{
    protected EnemyStats enemyStats;
    protected int playerLayer;

    protected virtual void Awake() {
        enemyStats = GetComponent<EnemyStats>();
        playerLayer = LayerMask.NameToLayer("Player");
    }
}
