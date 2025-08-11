using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = -90f;
    public float speed = 2f;
    public KeyCode interactKey = KeyCode.E;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool playerInRange = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
        }

        // Smoothly rotate
        if (isOpen)
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * speed);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}

