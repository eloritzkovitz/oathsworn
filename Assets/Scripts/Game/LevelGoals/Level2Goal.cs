using UnityEngine;
using System.Collections.Generic;

public class Level2Goal : MonoBehaviour, ILevelGoal
{
    private int checkpointsToVisit;
    private int checkpointsVisited = 0;
    private HashSet<Checkpoint> visitedCheckpoints = new HashSet<Checkpoint>();

    private GoalIndicatorUI goalIndicator;    
    [SerializeField] private Checkpoint[] checkpoints;

    private void Start()
    {
        checkpointsToVisit = checkpoints.Length;
        goalIndicator = FindFirstObjectByType<GoalIndicatorUI>();
        UpdateGoalIndicator();
    }

    // Called when a checkpoint is visited
    public void OnCheckpointVisited(Checkpoint checkpoint)
    {
        // Only count if this checkpoint is in the assigned list
        if (System.Array.IndexOf(checkpoints, checkpoint) >= 0 && !visitedCheckpoints.Contains(checkpoint))
        {
            Debug.Log($"Checkpoint visited: {checkpoint.name}");
            visitedCheckpoints.Add(checkpoint);
            checkpointsVisited++;
            UpdateGoalIndicator();

            if (checkpointsVisited >= checkpointsToVisit)
            {
                OnGoalCompleted();
            }
        }
    }

    // Updates the goal indicator UI
    public void UpdateGoalIndicator()
    {
        bool completed = checkpointsVisited >= checkpointsToVisit;
        if (goalIndicator != null)
            goalIndicator.SetGoal("Reach the castle. Checkpoints visited", checkpointsVisited, checkpointsToVisit, completed);
    }

    public void OnGoalCompleted()
    {
        LevelManager.Instance.CompleteCurrentLevel();
    }
}
