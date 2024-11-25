using UnityEngine;


public class GameConfigProvider : MonoBehaviour
{
    public static GameConfigProvider instance;
    private Config _gameConfig;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }

        _gameConfig = Resources.Load<Config>("GameConfig");

        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is missing in Resources folder!");
        }
    }
    public Config GameConfig => _gameConfig;
}
