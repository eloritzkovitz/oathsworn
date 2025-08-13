using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.lastCheckpoint = transform;
                Debug.Log("Checkpoint reached!");

                // When in scene 2, notify the goal
                if (SceneManager.GetActiveScene().name == "Scene2")
                {
                    var goal = FindFirstObjectByType<Level2Goal>();
                    if (goal != null)
                    {
                        goal.OnCheckpointVisited(this);
                    }
                }
            }
        }
    }
}
