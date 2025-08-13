using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalIndicatorUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text progressText;

    public void SetGoal(Sprite icon, int current, int total, bool completed)
    {
        iconImage.sprite = icon;
        progressText.text = $"{current}/{total}";
        Debug.Log($"GoalIndicatorUI updated: {current}/{total}");  
    }
}
