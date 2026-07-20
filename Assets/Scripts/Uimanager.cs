using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    [Header("HUD")]

    [SerializeField] private UnityEngine.UI.Image heartsDisplay;
    [SerializeField] private Sprite hearts0Sprite;

    [SerializeField] private Sprite hearts3Sprite;
    [SerializeField] private Sprite hearts2Sprite;
    [SerializeField] private Sprite hearts1Sprite;
    [SerializeField] private TextMeshProUGUI coinsText;


    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [Header("Win Panel")]
    [SerializeField] private GameObject winPanel;



    private void Start()
    {
        // Make sure panels are hidden at start
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }


    public void UpdateLives(int lives)
    {
        if (heartsDisplay == null) return;

        if (lives >= 3)
            heartsDisplay.sprite = hearts3Sprite;
        else if (lives == 2)
            heartsDisplay.sprite = hearts2Sprite;
        else if (lives <= 1)
            heartsDisplay.sprite = hearts1Sprite;
        else
            heartsDisplay.sprite = hearts0Sprite; // No hearts left
    }

    public void UpdateCoins(int current, int total)
    {
        if (coinsText != null)
            coinsText.text = "x " + current + "/" + total;
    }


    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }

    /// Shows the Win panel.
    /// Called by GameManager when coins reach target.
    public void ShowWin()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }


    public void RestartGame()
    {
        // Resume time first!
        Time.timeScale = 1f;
        GameManager.Instance.StartGame();
    }


    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}