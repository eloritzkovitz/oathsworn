using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        // Reset global game state
        if (GameSettings.Instance != null)
            GameSettings.Instance.ResetGame();

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

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}