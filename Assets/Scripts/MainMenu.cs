using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            PlayGame();
        }
    }

    public void PlayGame()
    {
        // Use GameManager to handle everything properly
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            // Fallback if no GameManager exists yet
            PlayerPrefs.SetInt("Lives", 3);
            PlayerPrefs.Save();
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level1");
        }
    }
}