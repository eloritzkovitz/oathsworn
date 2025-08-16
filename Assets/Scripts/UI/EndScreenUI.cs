using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Return to the main menu
    public void OnReturnToMainMenu()
    {
        SceneManager.LoadScene("GameLobby");
    }

    // Quit the game
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
