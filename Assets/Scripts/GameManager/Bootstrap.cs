using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Config _gameConfig;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private WinMenu _winMenu;
    [SerializeField] private GameObject _aboutMenu;

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _goldPrefab;
    [SerializeField] private GameObject _bagPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;

    private ResourceModel _resourceModel;
    private ResourcePresenter _resourcePresenter;
    private RewardManager _rewardManager;
    private GameObject _bagInstance;
    private SaverLoader _saveLoader;
    private DataBaseManager _dbManager;
    private int _currentUserId;

    private void Start()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap!");
            return;
        }

        _saveLoader = new SaverLoader();
        _dbManager = new DataBaseManager(Application.persistentDataPath +  "/game.db");
        
        //получаем текущий UserId из PlayerPrefs
        _currentUserId = PlayerPrefs.GetInt("UserId", -1);
        if (_currentUserId == -1)
        {
            Debug.LogError("No UserId found in PlayerPrefs!");
            InitializeGameFromConfig();
            return;
        }
        var dbData = LoadGameFromDataBase(_currentUserId);
        if (dbData != null)
        {
            Debug.Log("Load form db!");
            LoadGame(dbData);
        }
        else
        {
            // //если не удалось получить данные из бд, то загружаемся  с json
            // var savedData = _saveLoader.LoadGame();
            // if (savedData != null)
            // {
            //     Debug.Log("Load form json!");
            //     LoadGame(savedData);
            // }
            // else
            // {
            
                //если нет json, то берем параметры из config
                Debug.Log("Load form config!");
                InitializeGameFromConfig();
            // }
        }
        
    }

    private GameSaveData LoadGameFromDataBase(int userId)
    {
        try
        {
            Debug.Log("Attempting to load game state from database...");
            var dbData = _dbManager.LoadGame(userId);
            if (dbData == null)
            {
                Debug.LogWarning("No game state found in database for user.");
            }
            return dbData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game state from database: {e.Message}");
            return null;
        }
    }

    private void OnDisable()
    {
        UnbindEvents();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Логика игры
    private void InitializeGameFromConfig()
    {
        Debug.Log("Initializing game from Config...");
        StartGame();
        PlaceBagIfNeeded();
    }

    private void LoadGame(GameSaveData savedData)
    {
        Debug.Log("Loading game from save file...");

        InitializeResources(savedData.shovels, savedData.collectedGold);
        InitializeRewardManager();
        BindEvents();

        _gridManager.Initialize(_gameConfig.FieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resourcePresenter, _rewardManager);
        _gridManager.HandleGenerateGrid(savedData.cells);
        CenterCamera(_gridManager.GetGridCenter());
        PlaceBagIfNeeded();

        Debug.Log("Game successfully loaded.");
    }

    private void SaveGame()
    {
        var saveData = new GameSaveData
        {
            shovels = _resourceModel.ShovelCount,
            collectedGold = _resourceModel.CollectedGold,
            cells = _gridManager.GetCellData()
        };
        
        //сохраняем в json
        _saveLoader.SaveGame(saveData);
        
        //сохраняем в бд
        _dbManager.SaveGame(_currentUserId, saveData);
        
        Debug.Log("Game saved.");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        UnbindEvents();
        _winMenu.gameObject.SetActive(false);
        _rewardManager.ClearAllGold();
        _gridManager.ClearGrid();

        _saveLoader.DeleteSaveFile();
        //_dbManager.SaveGame(_currentUserId, null); // Сбрасываем состояние игры в БД

        InitializeGameFromConfig();
    }
    
    public void AboutGame()
    {
        Debug.Log("About game...");
        _aboutMenu.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        InitializeResources(_gameConfig.InitialShovelCount, 0);
        InitializeRewardManager();

        _gridManager.Initialize(_gameConfig.FieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resourcePresenter, _rewardManager);
        _gridManager.GenerateGrid();
        BindEvents();
        CenterCamera(_gridManager.GetGridCenter());
    }

    private void InitializeResources(int shovels, int collectedGold)
    {
        _resourceView.Init(shovels, _gameConfig.RequiredGoldBars, collectedGold);

        if(_resourceModel == null)
        {
            _resourceModel = new ResourceModel(_gameConfig.RequiredGoldBars, collectedGold, shovels);
        } 
        else
        {
            _resourceModel.ResetModel(_gameConfig.RequiredGoldBars, collectedGold, shovels);
        }
        
        _resourcePresenter = new ResourcePresenter(_resourceModel, _resourceView);
        BindEvents();
    }

    private void InitializeRewardManager()
    {
        if (_rewardManager == null)
        {
            _rewardManager = new RewardManager(_gameConfig.GoldSpawnChance, _gameConfig.GoldSpawnChanceIncrement, _goldPrefab, _resourcePresenter);
        }
    }

    private void PlaceBagIfNeeded()
    {
        if (_bagInstance == null)
        {
            PlaceBag();
        }
    }

    private void PlaceBag()
    {
        float bagOffset = 1f;
        Vector2 gridCenter = _gridManager.GetGridCenter();
        Vector2 bagPosition = new Vector2(gridCenter.x, gridCenter.y - _gameConfig.FieldSize / 2f - bagOffset);

        _bagInstance = Instantiate(_bagPrefab, bagPosition, Quaternion.identity);
    }

    private void CenterCamera(Vector2 gridCenter)
    {
        if (Camera.main != null) Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);
    }

    private void BindEvents()
    {
        _resourcePresenter.OnGameWon += HandleGameWon;
    }

    private void UnbindEvents()
    {
        _resourcePresenter.OnGameWon -= HandleGameWon;
    }

    private void HandleGameWon(int collectedGold)
    {
        Debug.Log("Game Won!");
        _winMenu.Setup(collectedGold);
    }
}
