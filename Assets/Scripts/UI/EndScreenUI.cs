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

    public void ShowEndScreen()
    {
        // Activate this UI
        gameObject.SetActive(true);

        // Disable other UI canvases
        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (canvas.gameObject != gameObject)
                canvas.gameObject.SetActive(false);
        }
 
        // Disable player controls
        var player = GameObject.FindWithTag("Player");
        if (player != null)
            player.SetActive(false);

        // Disable enemies
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            enemy.SetActive(false);
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
