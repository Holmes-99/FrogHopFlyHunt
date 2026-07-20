using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class DeathZone : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Frog fell in water!");

            // Camera shake!
            if (impulseSource != null)
                impulseSource.GenerateImpulse();

            // Play death sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayDeath();

            // Disable player
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
                player.DisablePlayer();

            StartCoroutine(DeathDelay());
        }
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife();
    }
}