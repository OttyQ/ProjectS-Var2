using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [Header("Config and Managers")]
    [SerializeField] private Config _gameConfig;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private WinMenu _winMenu;
    [SerializeField] private GameObject _aboutMenu;

    [Header("Game Prefabs")]
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
    private GameProgression _gameProgression;
    private int _currentUserId;

    // Unity Methods
    private void Start()
    {
        if (!ValidateConfig())
            return;

        InitializeManagers();
        if (!TryLoadGame())
        {
            InitializeGameFromConfig();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnDisable()
    {
        UnbindEvents();
    }

    // Public Methods
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        ResetGameScene();
        StartGameWithProgression();
    }

    public void NextLevel()
    {
        _gameProgression.IncreaseProgression();
        RestartGame();
    }

    public void AboutGame()
    {
        Debug.Log("Opening About Menu...");
        _aboutMenu.SetActive(true);
    }

    public void Logout()
    {
        Debug.Log("Logging out...");
        SaveGame();
        ClearUserData();
        SceneManager.LoadScene("Start");
    }

    // Private Methods
    private bool ValidateConfig()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap!");
            return false;
        }
        return true;
    }

    private void InitializeManagers()
    {
        _saveLoader = new SaverLoader();
        _dbManager = new DataBaseManager(Application.persistentDataPath + "/game.db");
        _gameProgression = new GameProgression(_gameConfig);
        _currentUserId = PlayerPrefs.GetInt("UserId", -1);

        if (_currentUserId == -1)
        {
            Debug.LogError("No UserId found in PlayerPrefs!");
        }
    }

    private bool TryLoadGame()
    {
        if (_currentUserId == -1)
            return false;

        var dbData = _dbManager.LoadGame(_currentUserId);
        if (dbData != null)
        {
            Debug.Log("Loaded game from database.");
            LoadGame(dbData);
            return true;
        }

        Debug.Log("No game state found in database.");
        return false;
    }

    private void LoadGame(GameSaveData savedData)
    {
        Debug.Log("Loading game...");
        _gameProgression.InitializeFromSave(savedData);
        InitializeResources(savedData.shovels, savedData.requiredGoldBars, savedData.collectedGold);
        InitializeRewardManager();

        SetupGridManager(savedData.cells);
        SetupGameScene();

        Debug.Log("Game loaded successfully.");
    }

    private void SaveGame()
    {
        var saveData = new GameSaveData
        {
            shovels = _resourceModel.ShovelCount,
            maxShovels =  _gameProgression.MaxShovelCount,
            collectedGold = _resourceModel.CollectedGold,
            cells = _gridManager.GetCellData(),
            maxDepth = _gameProgression.MaxDepth,
            fieldSize = _gameProgression.FieldSize,
            requiredGoldBars = _gameProgression.RequiredGoldBars,
            goldSpawnChance = _gameProgression.GoldSpawnChance,
            goldSpawnChanceIncrement = _gameProgression.GoldSpawnChanceIncrement
        };

        _saveLoader.SaveGame(saveData);
        _dbManager.SaveGame(_currentUserId, saveData);

        Debug.Log("Game saved successfully.");
    }

    private void InitializeGameFromConfig()
    {
        Debug.Log("Initializing game from Config...");
        _gameProgression.ResetProgression();
        StartGameWithProgression();
    }

    private void StartGameWithProgression()
    {
        InitializeResources(_gameProgression.MaxShovelCount, _gameProgression.RequiredGoldBars, 0);
        InitializeRewardManager();
        SetupGridManager(null);
        SetupGameScene();

        Debug.Log("Game started with progression.");
    }

    private void InitializeResources(int shovels, int requiredGoldBars, int collectedGold)
    {
        _resourceView.Init(shovels, requiredGoldBars, collectedGold);

        if (_resourceModel == null)
        {
            _resourceModel = new ResourceModel(requiredGoldBars, collectedGold, shovels);
        }
        else
        {
            _resourceModel.ResetModel(requiredGoldBars, collectedGold, shovels);
        }

        _resourcePresenter = new ResourcePresenter(_resourceModel, _resourceView);
        BindEvents();
    }

    private void InitializeRewardManager()
    {
        if (_rewardManager == null)
        {
            _rewardManager = new RewardManager(
                _gameProgression.GoldSpawnChance,
                _gameProgression.GoldSpawnChanceIncrement,
                _goldPrefab,
                _resourcePresenter
            );
        }
    }

    private void SetupGridManager(List<CellData> cellsData)
    {
        _gridManager.Initialize(
            _gameProgression.FieldSize,
            _cellPrefab,
            _boardPrefab,
            _gameProgression.MaxDepth,
            _resourcePresenter,
            _rewardManager
        );

        if (cellsData != null)
        {
            _gridManager.HandleGenerateGrid(cellsData, _gameProgression.MaxDepth);
        }
        else
        {
            _gridManager.GenerateGrid();
        }
    }

    private void SetupGameScene()
    {
        //ResetGameScene();
        CenterCamera(_gridManager.GetGridCenter());
        PlaceBagIfNeeded();
    }

    private void ResetGameScene()
    {
        _winMenu.gameObject.SetActive(false);
        _rewardManager?.ClearAllGold();
        _gridManager?.ClearGrid();
        
        UnbindEvents();
        
        _rewardManager = null;
        
        if (_bagInstance != null)
        {
            Destroy(_bagInstance);
            _bagInstance = null;
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
        Vector2 bagPosition = new Vector2(gridCenter.x, gridCenter.y - _gameProgression.FieldSize / 2f - bagOffset);

        _bagInstance = Instantiate(_bagPrefab, bagPosition, Quaternion.identity);
    }

    private void CenterCamera(Vector2 gridCenter)
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);
        }
    }

    private void ClearUserData()
    {
        _gameProgression = null;
        PlayerPrefs.DeleteKey("UserId");
        PlayerPrefs.Save();
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
