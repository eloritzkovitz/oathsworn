using UnityEngine;

public class GateController : MonoBehaviour
{
    public Transform gate;
    public Vector3 targetPositionOffset = new Vector3(0, 5f, 0);
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    private AudioSource audioSource;
    private AudioClip gateOpenSound;
    private bool soundPlayed = false;

    void Start()
    {
        closedPosition = gate.position;
        openPosition = closedPosition + targetPositionOffset;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        gateOpenSound = Resources.Load<AudioClip>("Audio/SFX/gate-opening");
    }

    void Update()
    {
        if (isOpening)
        {
            gate.position = Vector3.MoveTowards(gate.position, openPosition, openSpeed * Time.deltaTime);

            // Play sound once when opening starts
            if (!soundPlayed && gateOpenSound != null)
            {
                audioSource.PlayOneShot(gateOpenSound);
                soundPlayed = true;
            }
        }
    }

    public void OpenGate()
    {
        isOpening = true;        
    }
}
