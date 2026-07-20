using UnityEngine;
using System.Collections;

public class WinZone : MonoBehaviour
{
    [SerializeField] private GameObject winParticlePrefab;
    private Animator chestAnimator;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("YOU WIN!");

            // Open chest animation
            if (chestAnimator != null)
                chestAnimator.SetBool("isOpening", true);

            // Spawn win particles at chest position
            if (winParticlePrefab != null)
                Instantiate(winParticlePrefab, transform.position, Quaternion.identity);

            StartCoroutine(WinDelay());
        }
    }

    private IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        if (UIManager.Instance != null)
            UIManager.Instance.ShowWin();
    }
}