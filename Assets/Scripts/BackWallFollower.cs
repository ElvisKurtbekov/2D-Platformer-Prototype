using UnityEngine;

public class BackWallFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offset = -8f;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 position = transform.position;
        float targetX = player.position.x + offset;

        if (position.x < targetX)
        {
            position.x = targetX;
            transform.position = position;
        }
    }
}