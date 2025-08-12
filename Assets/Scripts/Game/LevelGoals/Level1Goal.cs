using System.Collections;
using UnityEngine;

public class Level1Goal : MonoBehaviour, ILevelGoal
{    
    private int enemiesToKill;
    private int enemiesKilled = 0;       

    private void Start()
    {        
        int difficulty = GameSettings.Instance.difficultyLevel;
        enemiesToKill = difficulty + 1;
    }

    // Called when an enemy is killed
    public void OnEnemyKilled()
    {
        enemiesKilled++;
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
