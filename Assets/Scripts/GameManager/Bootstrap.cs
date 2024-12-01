using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Поля
    [SerializeField] private Config _gameConfig;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private WinMenu _winMenu;

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _goldPrefab;
    [SerializeField] private GameObject _bagPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;

    private ResourceModel _resourceModel;
    private ResourcePresenter _resourcePresenter;
    private RewardManager _rewardManager;
    private GameObject _bagInstance;
    private SaverLoader _saveLoader;

    // Unity методы
    private void Start()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap!");
            return;
        }

        _saveLoader = new SaverLoader();

        var savedData = _saveLoader.LoadGame();
        if (savedData != null)
        {
            LoadGame(savedData);
        }
        else
        {
            InitializeGameFromConfig();
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

        _saveLoader.SaveGame(saveData);
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

        InitializeGameFromConfig();
    }

    // Приватные методы
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
        Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);
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
