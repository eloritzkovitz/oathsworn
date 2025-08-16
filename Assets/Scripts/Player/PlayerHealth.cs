using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public HealthBarUI healthBar;
    public Transform lastCheckpoint;
    
    void Start()
    {
        currentHealth = maxHealth;
        
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
