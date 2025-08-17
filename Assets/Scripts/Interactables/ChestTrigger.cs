using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    public Animator chestAnimator;
    private bool isPlayerNear = false;

    private AudioSource audioSource;
    private AudioClip chestOpenSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load the audio clip from Resources (no file extension)
        chestOpenSound = Resources.Load<AudioClip>("Audio/SFX/chest-opening");
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            chestAnimator.SetTrigger("OpenChest");
            PlayChestOpenSound();
        }
    }

    // Play chest open sound
    private void PlayChestOpenSound()
    {
        if (chestOpenSound != null && audioSource != null)
            audioSource.PlayOneShot(chestOpenSound);
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

