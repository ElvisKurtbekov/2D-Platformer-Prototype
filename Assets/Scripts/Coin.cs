using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        AudioSource.PlayClipAtPoint(coinClip, transform.position);
        GameManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}