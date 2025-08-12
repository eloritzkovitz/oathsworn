using UnityEngine;

[System.Serializable]
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Header("Game Parameters")]
    public int difficultyLevel = -1; // -1 = not selected yet
    public int health = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Save to JSON string
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Load from JSON string
    public void FromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}