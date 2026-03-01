using UnityEngine;

public class ChunkCleaner : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float destroyDistance = 30f;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = player.position.x - transform.position.x;

        if (distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}