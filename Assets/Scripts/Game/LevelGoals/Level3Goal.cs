using UnityEngine;

public class Level3Goal : MonoBehaviour, ILevelGoal
{ 
    private GoalIndicatorUI goalIndicator;       
    [SerializeField] private Checkpoint victoryCheckpoint;    

    private bool checkpointReached = false;

    private void Start()
    {  
        goalIndicator = FindFirstObjectByType<GoalIndicatorUI>();      
        UpdateGoalIndicator();
    }

    // Called when the victory checkpoint is triggered
    public void OnCheckpointVisited(Checkpoint checkpoint)
    {
        if (!checkpointReached && checkpoint == victoryCheckpoint)
        {
            checkpointReached = true;
            UpdateGoalIndicator();
            OnGoalCompleted();
        }
    }

    // Updates the goal indicator UI
    public void UpdateGoalIndicator()
    {
        bool completed = checkpointReached;
        if (goalIndicator != null)
            goalIndicator.SetGoal("Find the lord", checkpointReached ? 1 : 0, 1, completed);
    }

    // Called when the goal is completed
    public void OnGoalCompleted()
    {
        GameManager.Instance.HandleVictory();
    }
}
