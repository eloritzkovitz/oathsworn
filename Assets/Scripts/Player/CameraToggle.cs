using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    void Start()
    {
        // Start in third person
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            bool isThirdPerson = thirdPersonCamera.enabled;
            thirdPersonCamera.enabled = !isThirdPerson;
            firstPersonCamera.enabled = isThirdPerson;
        }
    }
}