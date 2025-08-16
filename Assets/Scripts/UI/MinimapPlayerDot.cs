using UnityEngine;
using UnityEngine.UI;

public class MinimapPlayerDot : MonoBehaviour
{
    public Transform player;
    public RectTransform minimapRect;
    public Vector2 mapWorldSize = new Vector2(1000, 1000); 
    public Vector2 mapOrigin = Vector2.zero;

    private RectTransform dotRect;

    void Awake()
    {
        dotRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (player != null && minimapRect != null)
        {
            // Get player's position relative to map origin
            Vector2 playerPos = new Vector2(player.position.x, player.position.z);
            Vector2 relativePos = playerPos - mapOrigin;

            // Normalize position (0 to 1)
            Vector2 normalizedPos = new Vector2(
                Mathf.Clamp01(relativePos.x / mapWorldSize.x),
                Mathf.Clamp01(relativePos.y / mapWorldSize.y)
            );

            // Swap axes and invert X for Y rotation 270
            Vector2 minimapSize = minimapRect.sizeDelta;
            Vector2 uiPos = new Vector2(
                (normalizedPos.y - 0.5f) * minimapSize.x,
                -(normalizedPos.x - 0.5f) * minimapSize.y   
            );

             dotRect.anchoredPosition = uiPos;
        }
    }
}