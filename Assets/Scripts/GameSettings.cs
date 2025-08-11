using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public int difficultyLevel = -1; // -1 = not selected yet

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
}
