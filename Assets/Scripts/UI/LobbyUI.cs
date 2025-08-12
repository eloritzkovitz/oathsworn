using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{    
    // Panels
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    
    // Game buttons
    public Button startGameButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    // Set difficulty level to -1 initially (not selected)
    private int difficultyLevel = -1;   

    private void Start()
    {
        // Show main menu first
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        startGameButton.interactable = false;        
    }

    // Called by New Game button
    public void OnNewGame()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);        
    }

    // Called by difficulty buttons (Easy/Medium/Hard)
    public void OnSelectDifficulty(int level)
    {
        difficultyLevel = level;
        startGameButton.interactable = true; // enable Start Game

        // Set selection state for each button
        easyButton.GetComponent<ButtonTextHighlighter>().SetSelected(level == 0);
        mediumButton.GetComponent<ButtonTextHighlighter>().SetSelected(level == 1);
        hardButton.GetComponent<ButtonTextHighlighter>().SetSelected(level == 2);
    }

    // Called by Start Game button
    public void OnStartGame()
    {
        GameSettings.Instance.difficultyLevel = difficultyLevel;
        SceneManager.LoadScene("MainScene");        
    }

    // Called by Back button
    public void OnBackToMenu()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    // Quit button
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
