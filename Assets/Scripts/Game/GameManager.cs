using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private List<Sprite> levelLoadingImages;    
    [SerializeField] private List<string> levelLoadingMessages; 

    public Canvas endScreenCanvas;
    public UnityEngine.UI.Image endScreenImage;
    public TMPro.TextMeshProUGUI endScreenTitle;
    public TMPro.TextMeshProUGUI endScreenMessage;
    public Sprite victorySprite;
    public Sprite defeatSprite;

    public int playerHealth = 5;

    // Global parameter for current gameplay scene
    public string CurrentGameplayScene { get; private set; }

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

    // Start a new game
    public void StartNewGame()
    {
        // Destroy all existing players
        foreach (var player in Object.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None))
        {
            Destroy(player.gameObject);
        }

        // Load MainScene
        LoadSceneAsync("MainScene");
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

    // Load a scene asynchronously (single)
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    // Coroutine to load a scene asynchronously (single)
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // Show loading screen
        if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingScreen != null)
            LoadingScreenManager.Instance.loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float minLoadingTime = 5f;
        float elapsedTime = 0f;

        // Update progress bar and wait for both scene load and minimum time
        while (asyncLoad.progress < 0.9f || elapsedTime < minLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingSlider != null)
                LoadingScreenManager.Instance.loadingSlider.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;

        // Hide loading screen
        if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingScreen != null)
            LoadingScreenManager.Instance.loadingScreen.SetActive(false);

        CurrentGameplayScene = sceneName;
    }

    // Load a scene asynchronously (additive, for levels)
    public void LoadLevelAdditive(string sceneName, int index, System.Action onLoaded = null)
    {
        StartCoroutine(LoadLevelAdditiveCoroutine(sceneName, index, onLoaded));
    }

    private IEnumerator LoadLevelAdditiveCoroutine(string sceneName, int index, System.Action onLoaded)
    {
        // Show loading screen
        if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingScreen != null)
            LoadingScreenManager.Instance.loadingScreen.SetActive(true);

        // Set custom loading image/text if available
        if (LoadingScreenManager.Instance != null)
        {
            if (levelLoadingImages != null && index < levelLoadingImages.Count && LoadingScreenManager.Instance.loadingImage != null)
                LoadingScreenManager.Instance.loadingImage.sprite = levelLoadingImages[index];
            if (levelLoadingMessages != null && index < levelLoadingMessages.Count && LoadingScreenManager.Instance.loadingText != null)
                LoadingScreenManager.Instance.loadingText.text = levelLoadingMessages[index];
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        float minLoadingTime = 5f;
        float elapsedTime = 0f;

        while (asyncLoad.progress < 0.9f || elapsedTime < minLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingSlider != null)
                LoadingScreenManager.Instance.loadingSlider.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;

        // Hide loading screen
        if (LoadingScreenManager.Instance != null && LoadingScreenManager.Instance.loadingScreen != null)
            LoadingScreenManager.Instance.loadingScreen.SetActive(false);

        CurrentGameplayScene = sceneName;

        // Callback for LevelManager to assign minimap/player
        onLoaded?.Invoke();
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
        DisableGameplayObjects();
    }

    // Handle defeat conditions
    public void HandleDefeat()
    {
        Debug.Log("Defeat!");
        string title = "You are dead!";
        string message = "You have died, and with you the hope to save the realm. All is now lost.";
        ShowEndScreen(title, message, false);
        DisableGameplayObjects();
    }

    // Show the end screen UI with the given title and message
    public void ShowEndScreen(string title, string message, bool isVictory)
    {
        // Disable all canvases except the end screen
        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (canvas != endScreenCanvas)
                canvas.gameObject.SetActive(false);
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

    // Disable gameplay objects and controls
    private void DisableGameplayObjects()
    {
        // Disable all player objects
        foreach (var player in Object.FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None))
        {
            player.gameObject.SetActive(false);
        }

        // Disable all enemy objects
        foreach (var enemy in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        { 
            enemy.gameObject.SetActive(false);
        }       
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}