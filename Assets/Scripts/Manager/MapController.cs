using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MapController : MonoBehaviour {
    [Header("配置")]
    [SerializeField] private GameObject chunkPrefab;//之后可以通过ChunkSO在游戏开始时生成不同的区块
    [SerializeField] private int chunkSize;
    [SerializeField] private int loadRange;

    [Header("性能优化")]
    [Tooltip("每帧最多处理多少个区块的操作")]
    [SerializeField] private int maxOpsPerFrame;

    private Vector2Int currentChunkCoord;
    private Vector2Int lastChunkCoord;
    private Dictionary<Vector2Int, GameObject> activeChunks= new Dictionary<Vector2Int, GameObject>();

    // 用于记录正在运行的协程，防止冲突
    private Coroutine updateCoroutine;

    private void Start() {
        currentChunkCoord = GetChunkCoordFromPosition(PlayerStats.Instance.transform.position);
        lastChunkCoord = currentChunkCoord;
        UpdateChunks();
    }

    private void Update() {
        if (PlayerStats.Instance == null) return;

        currentChunkCoord = GetChunkCoordFromPosition(PlayerStats.Instance.transform.position);

        if (currentChunkCoord != lastChunkCoord) {
            UpdateChunks();
            lastChunkCoord = currentChunkCoord;
        }
    }

    private Vector2Int GetChunkCoordFromPosition(Vector3 position) {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / chunkSize),
            Mathf.FloorToInt(position.y / chunkSize)
        );
    }

    private void UpdateChunks() {
        //如果上一次的协程还没跑完，玩家又移动了，先停止上一次的，避免重复生成或报错
        if (updateCoroutine != null) {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateChunkCoroutine());
    }

    private IEnumerator UpdateChunkCoroutine() {
        int opsCount = 0; // 操作计数器

        for (int x = -loadRange; x <= loadRange; x++) {
            for (int y = -loadRange; y <= loadRange; y++) {
                Vector2Int chunkCoord = new Vector2Int(currentChunkCoord.x + x, currentChunkCoord.y + y);

                if (!activeChunks.ContainsKey(chunkCoord)) {
                    SpawnChunk(chunkCoord);

                    opsCount++;

                    //如果这帧处理的数量超过了限制，就暂停，下一帧继续
                    if (opsCount >= maxOpsPerFrame) {
                        opsCount = 0; // 重置计数器
                        yield return null; // 等待下一帧（Update循环结束后）
                    }
                }
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (var key in activeChunks.Keys) {
            if (Mathf.Abs(key.x - currentChunkCoord.x) > loadRange || Mathf.Abs(key.y - currentChunkCoord.y) > loadRange) {
                chunksToRemove.Add(key);
            }
        }

        // 分帧移除
        foreach (var coord in chunksToRemove) {
            if (activeChunks.ContainsKey(coord)) {
                ObjectPoolManager.Instance.ReturnToPool(activeChunks[coord], chunkPrefab);
                activeChunks.Remove(coord);

                opsCount++;
                if (opsCount >= maxOpsPerFrame) {
                    opsCount = 0;
                    yield return null;
                }
            }
        }

        updateCoroutine = null;//表示该任务做完了，而不是当前帧携程结束
    }

    private void SpawnChunk(Vector2Int chunkCoord) {
        GameObject chunk =ObjectPoolManager.Instance.Spawn(chunkPrefab, new Vector2(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize), Quaternion.identity);
        activeChunks.Add(chunkCoord, chunk);
    }
}
