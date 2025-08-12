using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image[] healthHearts;
    [SerializeField] private Sprite fullHeart;     
    [SerializeField] private Sprite emptyHeart;  

    public void SetHealth(int health)
    {
        for (int i = 0; i < healthHearts.Length; i++)
        {
            healthHearts[i].sprite = i < health ? fullHeart : emptyHeart;
        }
    }
}
