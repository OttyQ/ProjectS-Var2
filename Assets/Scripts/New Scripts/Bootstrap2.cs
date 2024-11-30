using System;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap2 : MonoBehaviour
{
    [SerializeField] private Config _gameConfig;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceView _resourceView;
    [SerializeField] private WinMenu _winMenu;

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _goldPrefab;
    [SerializeField] private GameObject _bagPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;

    private ResourceModel _resModel;
    private ResourcePresenter _resPresenter;
    private RewardManager _rewardManager;
    private GameObject _bagInstance;
    private SaverLoader _saveLoader;

    void Start()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap2!");
            return;
        }

        _saveLoader = new SaverLoader(); // создаем SaverLoader

        var savedData = _saveLoader.LoadGame(); // Пытаемся загрузить игру
        if (savedData != null)
        {
            LoadGame(savedData); // Загружаем игру, если сохранение найдено
        }
        else
        {
            InitializeGameFromConfig(); // Инициализируем игру из Config, если сохранения нет
        }
    }

    private void OnDisable()
    {
        UnBind();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void InitializeGameFromConfig()
    {
        Debug.Log("Initializing game from Config...");
        StartGame();
        PlaceBagIfNeeded();
    }

    private void LoadGame(GameSaveData savedData)
    {
        Debug.Log("Loading game from save file...");

        // Восстанавливаем данные ресурсов
        _resModel = new ResourceModel(_gameConfig.requiredGoldBars, savedData.collectedGold, savedData.shovels);
        _resourceView.Init(savedData.shovels, _gameConfig.requiredGoldBars, savedData.collectedGold);
        _resPresenter = new ResourcePresenter(_resModel, _resourceView);
        Bind();

        // Восстанавливаем данные сетки
        _rewardManager = new RewardManager(_gameConfig.goldSpawnChance, _gameConfig.goldSpawnChanceIncrement, _goldPrefab, _resPresenter);
        _gridManager.Initialize(_gameConfig.fieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resPresenter, _rewardManager);
        _gridManager.HandleGenerateGrid(savedData.cells);
        CenterCamera(_gridManager.GetGridCenter());
        PlaceBagIfNeeded();

        Debug.Log("Game successfully loaded.");
    }

    private void SaveGame()
    {
        var saveData = new GameSaveData
        {
            shovels = _resModel.ShovelCount,
            collectedGold = _resModel.CollectedGold,
            cells = _gridManager.GetCellData()
        };

        _saveLoader.SaveGame(saveData);
        Debug.Log("Game saved.");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        UnBind();
        _winMenu.gameObject.SetActive(false);
        _rewardManager.ClearAllGold();
        _gridManager.ClearGrid();

        _saveLoader.DeleteSaveFile(); // Удаляем файл сохранения

        InitializeGameFromConfig(); // Перезапуск из Config
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
        Vector2 bagPosition = new Vector2(gridCenter.x, gridCenter.y - _gameConfig.fieldSize / 2f - bagOffset);

        _bagInstance = Instantiate(_bagPrefab, bagPosition, Quaternion.identity);
    }

    private void Bind()
    {
        _resPresenter.OnGameWon += HandleGameWon;
    }

    private void UnBind()
    {
        _resPresenter.OnGameWon -= HandleGameWon;
    }

    private void HandleGameWon(int collectedGold)
    {
        Debug.Log("Game Won!");
        _winMenu.Setup(collectedGold);
    }

    private void StartGame()
    {
        // Инициализация ресурсов
        ResourceInit();

        // Создаем RewardManager, если его еще нет
        if (_rewardManager == null)
        {
            _rewardManager = new RewardManager(_gameConfig.goldSpawnChance, _gameConfig.goldSpawnChanceIncrement, _goldPrefab, _resPresenter);
        }

        // Инициализируем и генерируем сетку
        _gridManager.Initialize(_gameConfig.fieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resPresenter, _rewardManager);
        _gridManager.GenerateGrid();

        // Центрируем камеру
        Vector2 gridCenter = _gridManager.GetGridCenter();
        CenterCamera(gridCenter);
    }

    private void ResourceInit()
    {
        _resourceView.Init(_gameConfig.initialShovelCount, _gameConfig.requiredGoldBars);

        if (_resModel == null)
        {
            _resModel = new ResourceModel(_gameConfig.requiredGoldBars, 0, _gameConfig.initialShovelCount);
        }

        if (_resPresenter == null)
        {
            _resPresenter = new ResourcePresenter(_resModel, _resourceView);
        }
        _resModel.ResetModel(_gameConfig.requiredGoldBars, 0, _gameConfig.initialShovelCount);
        Bind();
    }

    private void CenterCamera(Vector2 gridCenter)
    {
        Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);
    }
}
