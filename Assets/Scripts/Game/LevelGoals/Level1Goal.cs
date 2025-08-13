using System.Collections;
using UnityEngine;

public class Level1Goal : MonoBehaviour, ILevelGoal
{    
    private int enemiesToKill;
    private int enemiesKilled = 0;

    private GoalIndicatorUI goalIndicator;
    [SerializeField] private Sprite goalIcon;       

    private void Start()
    {
        // Set enemiesToKill based on difficulty
        enemiesToKill = GameSettings.Instance.difficultyLevel + 1;

        // Find the goal indicator UI
        goalIndicator = FindFirstObjectByType<GoalIndicatorUI>();

        // Update the UI at start
        UpdateGoalIndicator();
    }    

    // Updates the goal indicator UI
    public void UpdateGoalIndicator()
    {
        bool completed = enemiesKilled >= enemiesToKill;        
        if (goalIndicator != null)
            goalIndicator.SetGoal(goalIcon, enemiesKilled, enemiesToKill, completed);
    }

    // Called when an enemy is killed
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        UpdateGoalIndicator();
        if (enemiesKilled >= enemiesToKill)
        {            
            OnGoalCompleted();
        }
    }    

    // Called when the goal is completed
    public void OnGoalCompleted()
    {
        LevelManager.Instance.CompleteCurrentLevel();
    }    
}
