using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<string> levelSceneNames;
    [SerializeField] private Text messageText;
    [SerializeField] private float delayBeforeNextLevel = 2f;
    private int currentLevelIndex = 0;

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

    // Called when the level starts
    public void OnLevelStart()
    {
        if (levelSceneNames.Count > 0 && !SceneManager.GetSceneByName(levelSceneNames[0]).isLoaded)
        {
            LoadLevel(0);
        }
    }

    // Load a specific level by index
    public void LoadLevel(int index)
    {
        string sceneName = levelSceneNames[index];
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Debug.Log($"Loaded level: {sceneName}");
            currentLevelIndex = index;            
        }
        else
        {
            Debug.LogWarning($"Scene {sceneName} is already loaded.");
        }
    }    

    // Called when the level is completed
    public void CompleteCurrentLevel()
    {
        StartCoroutine(ShowMessageAndLoadNext());
    }    

    private IEnumerator ShowMessageAndLoadNext()
    {
        // Show UI message
        if (messageText != null)
        {
            messageText.text = "Level Complete!";
            messageText.enabled = true;
        }

        // Unload current level
        string currentScene = levelSceneNames[currentLevelIndex];
        SceneManager.UnloadSceneAsync(currentScene);

        // Wait for delay
        yield return new WaitForSeconds(delayBeforeNextLevel);

        // Hide message
        if (messageText != null)
        {
            messageText.enabled = false;
        }

        // Load next level if any
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelSceneNames.Count)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("All levels completed!");
            if (messageText != null)
            {
                messageText.text = "All levels complete!";
                messageText.enabled = true;
            }            
        }
    }
}