using UnityEngine;

public class GateController : MonoBehaviour
{
    public Transform gate;
    public Vector3 targetPositionOffset = new Vector3(0, 5f, 0); // Move up 5 units
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = gate.position;
        openPosition = closedPosition + targetPositionOffset;
    }

    void Update()
    {
        if (isOpening)
        {
            gate.position = Vector3.MoveTowards(gate.position, openPosition, openSpeed * Time.deltaTime);
        }
    }

    public void OpenGate()
    {
        isOpening = true;
    }
}
