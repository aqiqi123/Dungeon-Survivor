using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class EnemyMovementBatchCalculator : MonoBehaviour
{
    public static EnemyMovementBatchCalculator Instance { get; private set; }

    private readonly List<EnemyMovement> enemies = new List<EnemyMovement>(512);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Register(EnemyMovement enemy)
    {
        if (enemy == null || enemies.Contains(enemy))
            return;

        enemies.Add(enemy);
    }

    public void UnRegister(EnemyMovement enemy)
    {
        if (enemy == null)
            return;

        enemies.Remove(enemy);
    }

    private void FixedUpdate()
    {
        if (PlayerStats.Instance == null || enemies.Count == 0)
            return;

        int count = enemies.Count;

        NativeArray<float2> positions = new NativeArray<float2>(count, Allocator.TempJob);
        NativeArray<float> speeds = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<float2> desiredVelocities = new NativeArray<float2>(count, Allocator.TempJob);

        float2 playerPosition = new float2(
            PlayerStats.Instance.transform.position.x,
            PlayerStats.Instance.transform.position.y
        );

        try
        {
            for(int i = 0; i < count; i++)
            {
                EnemyMovement enemy = enemies[i];
                if (enemy == null)
                {
                    positions[i] = float2.zero;
                    speeds[i] = 0f;
                    continue;
                }

                positions[i] = enemy.Position;
                speeds[i] = enemy.Stats != null ? enemy.Stats.CurrentMoveSpeed : 0f;
            }

            var job = new EnemyMoveToPlayerJob
            {
                PlayerPosition = playerPosition,
                Positions = positions,
                Speeds=speeds,
                DesiredVelocities=desiredVelocities
            };

            //把count个工作按每64个一组分批处理
            job.Schedule(count, 64).Complete();

            for(int i = 0; i < count; i++)
            {
                EnemyMovement enemy = enemies[i];
                if (enemy == null)
                    continue;

                enemy.SetDesiredVelocity((Vector2)desiredVelocities[i]);
            }
        }
        finally
        {
            positions.Dispose();
            speeds.Dispose();
            desiredVelocities.Dispose();
        }
    }

    /// <summary>
    /// 并行计算每个敌人的期望速度
    /// </summary>
    [BurstCompile]
    private struct EnemyMoveToPlayerJob : IJobParallelFor {
        [ReadOnly] public NativeArray<float2> Positions;
        [ReadOnly] public NativeArray<float> Speeds;
        [ReadOnly] public float2 PlayerPosition;
        [WriteOnly] public NativeArray<float2> DesiredVelocities;

        public void Execute(int index)
        {
            float2 delta = PlayerPosition - Positions[index];
            float lenSq = math.lengthsq(delta);

            if (lenSq < 0.0001f)
            {
                DesiredVelocities[index] = float2.zero;
                return;
            }

            float2 dir = math.normalize(delta);
            DesiredVelocities[index] = dir * Speeds[index];
        }
    }
}
