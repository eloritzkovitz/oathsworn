using UnityEngine;

public class LootableWeapon : MonoBehaviour
{
    public WeaponData weaponData;

    private AudioSource audioSource;
    private AudioClip swordDrawSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load the audio clip from Resources
        swordDrawSound = Resources.Load<AudioClip>("Audio/SFX/sword-draw");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"You can pick up: {weaponData.weaponName} (Press F)");            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Pickup(other.gameObject);
        }
    }

    // Handle item pickup
    private void Pickup(GameObject player)
    {
        if (swordDrawSound != null && audioSource != null)
            audioSource.PlayOneShot(swordDrawSound);
        
        Debug.Log($"Picked up: {weaponData.weaponName}");        
        Destroy(gameObject);
    }
}

