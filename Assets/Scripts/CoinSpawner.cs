using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject prefab;
    public int minCount = 1;
    public int maxCount = 3;
    public float heightOffset = 0.5f;
    public float minDistance = 1.5f;
}

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private float raycastHeight = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpawnObject[] objectsToSpawn;
    [SerializeField] private Collider2D groundCollider;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return null;

        if (groundCollider == null)
        {
            Debug.LogError("Ground Collider не назначен!");
            yield break;
        }

        Bounds bounds = groundCollider.bounds;

        foreach (var obj in objectsToSpawn)
        {
            List<Vector2> localPositions = new List<Vector2>();
            int count = Random.Range(obj.minCount, obj.maxCount + 1);

            for (int i = 0; i < count; i++)
            {
                TrySpawn(obj, localPositions, bounds);
            }
        }
    }

    private void TrySpawn(SpawnObject obj, List<Vector2> localPositions, Bounds bounds)
    {
        for (int attempt = 0; attempt < 50; attempt++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);

            Vector2 rayStart = new Vector2(randomX, bounds.max.y + raycastHeight);

            RaycastHit2D hit = Physics2D.Raycast(
                rayStart,
                Vector2.down,
                raycastHeight * 3f,
                groundLayer
            );

            if (hit.collider == null) continue;

            Vector2 spawnPos = hit.point + Vector2.up * obj.heightOffset;

            bool tooClose = false;

            foreach (Vector2 pos in localPositions)
            {
                if (Vector2.Distance(pos, spawnPos) < obj.minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            Instantiate(obj.prefab, spawnPos, Quaternion.identity, transform);
            localPositions.Add(spawnPos);
            return;
        }

        Debug.LogWarning($"Не удалось заспавнить: {obj.prefab.name}");
    }
}