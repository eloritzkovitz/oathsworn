using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalIndicatorUI : MonoBehaviour
{    
    [SerializeField] private TMP_Text progressText;

    public void SetGoal(string text, int current, int total, bool completed)
    {        
        progressText.text = $"{text}: {current}/{total}";
        Debug.Log($"GoalIndicatorUI updated: {current}/{total}");
    }
}
