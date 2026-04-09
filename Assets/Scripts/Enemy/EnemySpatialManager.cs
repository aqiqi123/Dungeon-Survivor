using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpatialManager : MonoBehaviour
{
    public static EnemySpatialManager Instance { get; private set; }

    [Header("四叉树边界(中心在管理器物体位置)")]
    [SerializeField] private Vector2 worldSize = new Vector2(250f, 250f);

    [Header("四叉树参数")]
    [SerializeField] private int nodeCapacity = 12;
    [SerializeField] private int maxDepth = 6;

    [Header("分离参数")]
    [SerializeField] private float queryRadiusMultiplier = 2.5f;
    [SerializeField] private float separationStrength = 6f;
    [SerializeField] private float maxSeparationSpeed = 3f;

    private readonly List<EnemyAgent> activeEnemies = new List<EnemyAgent>(512);
    Dictionary<EnemyAgent, int> enemyIndexMap = new Dictionary<EnemyAgent, int>();
    private readonly List<EnemyAgent> queryBuffer = new List<EnemyAgent>(64);
    private QuadTreeNode tree;

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

        RebuildTree();
    }

    private void FixedUpdate()
    {
        if (tree == null)
            RebuildTree();

        tree.Clear();

        //把所有敌人放到树中
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            EnemyAgent e = activeEnemies[i];
            if (e != null)
                tree.Insert(e);
        }

        //碰撞检测
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            EnemyAgent self = activeEnemies[i];
            if (self == null)
                continue;

            Vector2 desiredVel = self.Em != null ? self.Em.DesiredVelocity : Vector2.zero;

            //设定查询框
            float selfRadius = Mathf.Max(0.001f, self.Radius);
            float queryR = selfRadius * queryRadiusMultiplier;
            Vector2 selfPos = self.Position;

            Rect queryRect = new Rect(
                selfPos.x - queryR,
                selfPos.y - queryR,
                queryR * 2f,
                queryR * 2f
            );

            //查找邻居
            queryBuffer.Clear();
            tree.Query(queryRect, queryBuffer);

            //计算推力
            Vector2 push = Vector2.zero;

            for (int j = 0; j < queryBuffer.Count; j++)
            {
                EnemyAgent other = queryBuffer[j];
                if (other == null || other == self)
                    continue;

                Vector2 delta = selfPos - other.Position;
                float distSq = delta.sqrMagnitude;
                float minDist = selfRadius + Mathf.Max(0.001f, other.Radius);
                float minDistSq = minDist * minDist;
                if (distSq >= minDistSq)
                    continue;

                // 完全重叠时给一个微小方向，防止 normalized 为零
                if (distSq < 0.000001f)
                {
                    delta = Random.insideUnitCircle.normalized * 0.001f;
                    distSq = delta.sqrMagnitude;
                }
                float dist = Mathf.Sqrt(distSq);
                Vector2 dir = delta / dist;
                float penetration = minDist - dist;
                push += dir * penetration;
            }

            //施加物理影响
            Vector2 sepVel = Vector2.zero;
            if (push.sqrMagnitude > 0f)
            {
                sepVel = Vector2.ClampMagnitude(push * separationStrength, maxSeparationSpeed);
            }

            self.Rb.velocity = desiredVel + sepVel;
        }
    }

    public void Register(EnemyAgent enemy)
    {
        if (enemy == null || enemyIndexMap.ContainsKey(enemy))
            return;
        int idx = activeEnemies.Count;
        activeEnemies.Add(enemy);
        enemyIndexMap.Add(enemy, idx);
    }

    public void UnRegister(EnemyAgent enemy)
    {
        if (enemy == null)
            return;
        if (!enemyIndexMap.TryGetValue(enemy, out int removeIdx))
            return;
        int lastIdx = activeEnemies.Count - 1;
        EnemyAgent last = activeEnemies[lastIdx];
        activeEnemies[removeIdx] = last;
        enemyIndexMap[last] = removeIdx;
        activeEnemies.RemoveAt(lastIdx);
        enemyIndexMap.Remove(enemy);
    }

    private void RebuildTree()
    {
        Vector2 center = transform.position;
        Rect worldRect = new Rect(
            center.x-worldSize.x*0.5f,
            center.y-worldSize.y*0.5f,
            worldSize.x,
            worldSize.y
        );

        tree = new QuadTreeNode(worldRect, nodeCapacity, maxDepth);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, worldSize);
    }
}
