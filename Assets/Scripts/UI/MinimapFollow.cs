using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 30, 0);

    // Keep the minimap persistent across scenes
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Follow the player's position
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y + offset.y, player.position.z);
        }
    }

    // Set the player transform
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}