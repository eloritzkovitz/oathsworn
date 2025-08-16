using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Canvas endScreenCanvas;
    public UnityEngine.UI.Image endScreenImage;
    public TMPro.TextMeshProUGUI endScreenTitle;
    public TMPro.TextMeshProUGUI endScreenMessage;
    public Sprite victorySprite;
    public Sprite defeatSprite;

    // Global parameter for current gameplay scene
    public string CurrentGameplayScene { get; private set; }

    // Start a new game
    public void StartNewGame()
    {
        // Destroy all existing players
        foreach (var player in Object.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None))
        {
            Destroy(player.gameObject);
        }

        // Load MainScene
        LoadScene("MainScene");
    }

    // Set the game difficulty
    public void SetDifficulty(int difficulty)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.difficultyLevel = difficulty;
            Debug.Log("Difficulty set in GameManager: " + difficulty);           
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load a scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        CurrentGameplayScene = sceneName;
    }

    // Load a scene by build index
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        CurrentGameplayScene = SceneManager.GetSceneByBuildIndex(buildIndex).name;
    }

    // Communicate with level managers
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset player state when the first gameplay scene is loaded
        if (scene.name == "Scene1")
        {
            PlayerHealth player = Object.FindFirstObjectByType<PlayerHealth>();
            if (player != null)
            {
                player.lastCheckpoint = null;
                player.currentHealth = player.maxHealth;            
            }
        }

        // Only trigger OnLevelStart for the MainScene
        if (scene.name == "MainScene")
        {
            // Reset checkpoints for all players
            PlayerHealth player = Object.FindFirstObjectByType<PlayerHealth>();
            if (player != null)
            {
                player.lastCheckpoint = null;
            }

            LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();
            if (levelManager != null)
            {
              levelManager.OnLevelStart();
            }
        }
    }

    // Handle victory conditions
    public void HandleVictory()
    {
        Debug.Log("Victory!");
        string title = "Victory!";
        string message = "You reached the lord and fulfilled your mission. With your valor, the prince was able to reclaim his throne. Peace has been restored!";
        ShowEndScreen(title, message, true);
    }

    // Handle defeat conditions
    public void HandleDefeat()
    {
        Debug.Log("Defeat!");
        string title = "You are dead!";
        string message = "You have died, and with you the hope to save the realm. All is now lost.";
        ShowEndScreen(title, message, false);

        // Disable player controls
        var playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerObj.SetActive(false);
        }
    }

    // Show the end screen UI with the given title and message
    public void ShowEndScreen(string title, string message, bool isVictory)
    {
        // Find and disable the main UI
        var mainScreenUI = GameObject.Find("UI");
        if (mainScreenUI != null)
        {
            mainScreenUI.SetActive(false);
        }

        // Enable the end screen UI        
        if (endScreenCanvas != null)
        {
            endScreenCanvas.gameObject.SetActive(true);

            endScreenTitle.text = title;
            endScreenMessage.text = message;
            endScreenImage.sprite = isVictory ? victorySprite : defeatSprite;
        }
        else
        {
            Debug.LogWarning("End screen canvas is not assigned!");
        }        
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}