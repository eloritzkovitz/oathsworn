using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Camera playerCamera;
    public float shootingRange = 100f;
    public GameObject hitEffect;
    public LineRenderer bulletTrail;
    public float trailDuration = 0.05f;
    public AudioSource shootingAudio;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    // Shooting logic
    void Shoot()
    {
        // Play the shooting sound
        if (shootingAudio != null)
        {
            shootingAudio.Play();
        }

        // Raycast from the camera to the mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, shootingRange))
        {
            if (hitEffect)
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));

            // Draw bullet trail
            StartCoroutine(ShowBulletTrail(ray.origin, hit.point));

            Debug.Log("Raycast hit: " + hit.collider.name);

            EnemyAI enemy = hit.collider.GetComponentInParent<EnemyAI>();
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                Debug.Log("Hit an enemy!");
                enemy.Die();
            }
        }
    }

    // Coroutine to show the bullet trail
    System.Collections.IEnumerator ShowBulletTrail(Vector3 start, Vector3 end)
    {
        bulletTrail.gameObject.SetActive(true);
        bulletTrail.SetPosition(0, start);
        bulletTrail.SetPosition(1, end);
        yield return new WaitForSeconds(trailDuration);
        bulletTrail.gameObject.SetActive(false);
    }
}
