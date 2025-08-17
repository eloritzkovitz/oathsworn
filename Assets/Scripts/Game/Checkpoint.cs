using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip checkpointSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load the audio clip from Resources
        checkpointSound = Resources.Load<AudioClip>("Audio/SFX/checkpoint");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.lastCheckpoint = transform;
                Debug.Log("Checkpoint reached!");

                // Play checkpoint sound
                if (checkpointSound != null && audioSource != null)
                    audioSource.PlayOneShot(checkpointSound);

                // Get the scene this checkpoint is in
                string checkpointScene = gameObject.scene.name;                

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
