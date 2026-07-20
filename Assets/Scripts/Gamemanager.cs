using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    [Header("Settings")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int coinsToWin = 5;

    // Current values during gameplay
    private int currentLives;
    private int currentCoins;

    private void Awake()
    {
        // If no GameManager exists, this becomes it
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // must be a root object for DontDestroyOnLoad to work
            DontDestroyOnLoad(gameObject); // survives scene changes
        }
        else
        {
            // If one already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    // Subscribe to scene loaded event
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe to prevent memory leaks
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This runs EVERY time a scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always unpause when ANY scene loads
        Time.timeScale = 1f;
        Debug.Log("Scene loaded: " + scene.name + " | Time.timeScale reset to 1");

        // If we loaded Level1, reset coin count for fresh start
        if (scene.name == "Level1")
        {
            currentCoins = 0;
            UpdateUI();
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (!PlayerPrefs.HasKey("Lives"))
        {
            currentLives = startingLives;
            PlayerPrefs.SetInt("Lives", currentLives);
            PlayerPrefs.Save();
        }
        else
        {
            currentLives = PlayerPrefs.GetInt("Lives", startingLives);
        }

        currentCoins = 0;
        UpdateUI();
    }

    /// Called by CoinCollectible when frog touches a coin.
    public void AddCoin()
    {
        currentCoins++;
        Debug.Log("Coins: " + currentCoins);

        UpdateUI();

        // Check win condition
        if (currentCoins >= coinsToWin)
            Win();
    }

    /// Called by DeathZone when frog falls in water.
    public void LoseLife()
    {
        currentLives--;
        Debug.Log("Lives remaining: " + currentLives);

        // Save BEFORE reloading!
        PlayerPrefs.SetInt("Lives", currentLives);
        PlayerPrefs.Save();

        UpdateUI();

        if (currentLives <= 0)
        {
            // Reset lives for next game
            PlayerPrefs.DeleteKey("Lives");
            GameOver();
        }
        else
            RestartLevel();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        currentLives = startingLives;
        currentCoins = 0;
        PlayerPrefs.SetInt("Lives", currentLives);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }

    public int GetCoins() => currentCoins;
    public int GetLives() => currentLives;
    public int GetCoinsToWin() => coinsToWin;

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER!");
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayLose();
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver();
    }

    private void Win()
    {
        Debug.Log("YOU WIN!");

        // Play win sound!
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        if (UIManager.Instance != null)
            UIManager.Instance.ShowWin();
    }

    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLives(currentLives);
            UIManager.Instance.UpdateCoins(currentCoins, coinsToWin);
        }
    }
}