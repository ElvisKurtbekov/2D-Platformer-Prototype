using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject[] tilemapPrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private float spawnDistance = 10f;

    [SerializeField] private GameObject currentTilemap;

    private void Update()
    {
        if (currentTilemap == null || player == null) return;

        Transform endPoint = currentTilemap.transform.Find("EndPoint");
        if (endPoint == null) return;

        float distance = Vector3.Distance(player.position, endPoint.position);

        if (distance < spawnDistance)
        {
            SpawnNextTilemap(endPoint.position);
        }
    }

    private void SpawnNextTilemap(Vector3 spawnWorldPos)
    {
        int index = Random.Range(0, tilemapPrefabs.Length);
        GameObject nextTilemap = Instantiate(tilemapPrefabs[index]);

        Transform startPoint = nextTilemap.transform.Find("StartPoint");

        if (startPoint == null)
        {
            Debug.LogWarning("Нет StartPoint в тайлмапе! Используем Transform объекта.");
            startPoint = nextTilemap.transform;
        }

        Vector3 offset = nextTilemap.transform.position - startPoint.position;
        nextTilemap.transform.position = spawnWorldPos + offset;
        nextTilemap.transform.SetParent(gridTransform);

        currentTilemap = nextTilemap;
    }
}