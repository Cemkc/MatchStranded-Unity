using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_Instance;

    [SerializeField] private GameSettings _settings;

    public static GameSettings Settings { get => s_Instance._settings; }

    void Awake()
    {
        if (s_Instance != null && s_Instance != this) 
        { 
            Destroy(this); 
        }
        else 
        { 
            s_Instance = this; 
        }
    }
}