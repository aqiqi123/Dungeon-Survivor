using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MapController : MonoBehaviour {
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int chunkSize;
    [SerializeField] private int loadRange;

    private Vector2Int currentChunkCoord;
    private Vector2Int lastChunkCoord;

    private Dictionary<Vector2Int, GameObject> activeChunks;
    private List<Vector2Int> chunksToRemove;

    private ObjectPool<GameObject> chunkPool;

    private void Awake() {
        activeChunks = new Dictionary<Vector2Int, GameObject>();
        chunksToRemove = new List<Vector2Int>();

        chunkPool = new ObjectPool<GameObject>(
            () => Instantiate(chunkPrefab, transform),
            (obj) => obj.SetActive(true),
            (obj) => obj.SetActive(false),
            (obj) => Destroy(obj),
            false,
            9,
            50
        );
    }

    private void Start() {
        UpdateChunks();
    }

    private void Update() {
        if (PlayerStats.instance == null) return;

        currentChunkCoord.x = Mathf.FloorToInt(PlayerStats.instance.transform.position.x / chunkSize);
        currentChunkCoord.y = Mathf.FloorToInt(PlayerStats.instance.transform.position.y / chunkSize);

        if (currentChunkCoord != lastChunkCoord) {
            UpdateChunks();
            lastChunkCoord = currentChunkCoord;
        }
    }

    private void OnDestroy() {
        chunkPool?.Dispose();
    }

    private void UpdateChunks() {
        for (int x = -loadRange; x <= loadRange; x++) {
            for (int y = -loadRange; y <= loadRange; y++) {
                Vector2Int chunkCoord = new Vector2Int(currentChunkCoord.x + x, currentChunkCoord.y + y);

                if (!activeChunks.ContainsKey(chunkCoord)) {
                    SpawnChunk(chunkCoord);
                }
            }
        }

        chunksToRemove.Clear();

        foreach (var key in activeChunks.Keys) {
            if (Mathf.Abs(key.x - currentChunkCoord.x) > loadRange || Mathf.Abs(key.y - currentChunkCoord.y) > loadRange) {
                chunksToRemove.Add(key);
            }
        }

        foreach (var coord in chunksToRemove) {
            chunkPool.Release(activeChunks[coord]);
            activeChunks.Remove(coord);
        }
    }

    private void SpawnChunk(Vector2Int chunkCoord) {
        GameObject chunk = chunkPool.Get();
        chunk.transform.position = new Vector3(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0);
        activeChunks.Add(chunkCoord, chunk);

        //之后可以编写MapSO存储所有的不同的chunk预组件，再编写Chunk.cs初始化区块
    }
}
