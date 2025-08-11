using UnityEngine;

public class LootableWeapon : MonoBehaviour
{
    public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"You can pick up: {weaponData.weaponName} (Press F)");
            // Show UI prompt here
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject player)
    {
        Debug.Log($"Picked up: {weaponData.weaponName}");
        // Option 1: Attach to player
        // Option 2: Add to inventory
        Destroy(gameObject); // Remove from world
    }
}

