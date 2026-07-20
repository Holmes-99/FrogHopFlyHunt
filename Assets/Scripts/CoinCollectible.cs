using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    [SerializeField] private GameObject collectParticlePrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Spawn particle at coin position
            if (collectParticlePrefab != null)
                Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCoin();

            GameManager.Instance.AddCoin();
            Destroy(gameObject);
        }
    }
}