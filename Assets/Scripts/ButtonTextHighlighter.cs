using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color highlightColor = new Color32(255, 214, 96, 255);
    public Color selectedColor = new Color32(212, 175, 67, 255);
    public Color defaultColor = new Color32(255, 255, 255, 255);

    private TextMeshProUGUI tmpText;
    private Color originalColor;
    private bool isSelected = false;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
            tmpText.color = defaultColor;
        originalColor = defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tmpText != null && !isSelected)
            tmpText.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tmpText != null && !isSelected)
            tmpText.color = defaultColor;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (tmpText == null) return;
        if (isSelected)
            tmpText.color = selectedColor;
        else
            tmpText.color = originalColor;
    }
}
