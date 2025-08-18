using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public HealthBarUI healthBar;
    public Transform lastCheckpoint;
    
    void Start()
    {
        // Use persistent health if available
        if (GameManager.Instance != null && GameManager.Instance.playerHealth > 0)
        {
            currentHealth = GameManager.Instance.playerHealth;
        }
        else
        {
            // Set starting health based on difficulty
            int difficulty = GameSettings.Instance != null ? GameSettings.Instance.difficultyLevel : 0;
            Debug.Log("Difficulty level: " + difficulty);
            switch (difficulty)
            {
                case 0: // Easy
                    currentHealth = 5;
                    break;
                case 1: // Medium
                    currentHealth = 3;
                    break;
                case 2: // Hard
                    currentHealth = 1;
                    break;
                default:
                    currentHealth = maxHealth;
                    break;
            }
            // Save initial health to GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = currentHealth;
            }
        }

        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<HealthBarUI>();
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthBarUI not found in the scene!");
        }
    }

    // Called when the player enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Player collided with water!");
            TakeDamage(1);
            ReturnToCheckpoint();
        }
    }

    // Called when the player takes damage
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage, health now: " + currentHealth);

        // Sync health with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth = currentHealth;
         }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthBarUI reference is missing!");
        }

        // Handle defeat when health reaches 0
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = currentHealth; // Ensure health is 0 in GameManager
                GameManager.Instance.HandleDefeat();
            }
            else
            {
                Debug.LogWarning("GameManager.Instance is null!");
            }
        }
    }

    // Called when the player respawns
    public void ReturnToCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // Disable before teleporting
                transform.position = lastCheckpoint.position;
                cc.enabled = true;  // Re-enable after teleporting
            }
            else
            {
                transform.position = lastCheckpoint.position;
            }
        }
    }
}
