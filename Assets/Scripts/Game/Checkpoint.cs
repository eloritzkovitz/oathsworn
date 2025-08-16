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

                // Get the scene this checkpoint is in
                string checkpointScene = gameObject.scene.name;
                //checkpointScene = "Scene2";

                if (checkpointScene == "Scene2")
                {
                    var goal = FindFirstObjectByType<Level2Goal>();
                    if (goal != null)
                    {
                        goal.OnCheckpointVisited(this);
                    }
                }
                else if (checkpointScene == "Scene3")
                {
                    var goal = FindFirstObjectByType<Level3Goal>();
                    if (goal != null)
                    {
                        goal.OnCheckpointVisited(this);
                    }
                }
            }
        }
    }
}
