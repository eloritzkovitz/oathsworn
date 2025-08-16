using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    public Animator chestAnimator;
    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            chestAnimator.SetTrigger("OpenChest");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Restore one health if below 5
                if (playerHealth.currentHealth < 5)
                {
                    playerHealth.currentHealth += 1;
                    playerHealth.healthBar.SetHealth(playerHealth.currentHealth);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}

