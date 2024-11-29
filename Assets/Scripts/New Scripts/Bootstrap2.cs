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

    void Start()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap2!");
            return;
        }

        InitializeGame();
    }


    private void InitializeGame()
    {
        

        StartGame();

        if (_bagInstance == null)
        {
            PlaceBag();
        }
    }

    private void StartGame()
    {
        // Инициализация ресурсов
        ResourceInit();

        if (_rewardManager == null)
        {
            _rewardManager = new RewardManager(_gameConfig.goldSpawnChance, _gameConfig.goldSpawnChanceIncrement, _goldPrefab, _resPresenter);
        }

        _gridManager.Initialize(_gameConfig.fieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resPresenter, _rewardManager);
        _gridManager.GenerateGrid();

        Vector2 gridCenter = _gridManager.GetGridCenter();
        CenterCamera(gridCenter);
    }

    private void ResourceInit()
    {
        // Инициализируем представление ресурсов
        _resourceView.Init(_gameConfig.initialShovelCount, _gameConfig.requiredGoldBars);

        // Инициализируем модель и презентер ресурсов (если они еще не созданы)
        if (_resModel == null)
        {
            _resModel = new ResourceModel(_gameConfig.initialShovelCount, 0, _gameConfig.requiredGoldBars);
        }

        if (_resPresenter == null)
        {
            _resPresenter = new ResourcePresenter(_resModel, _resourceView);
            
        }

        Bind();
        // Сбрасываем ресурсы к начальному состоянию
        _resModel.ResetModel(_gameConfig.requiredGoldBars, 0, _gameConfig.initialShovelCount);
    }

    public void RestartGame()
    {
        UnBind();
        _winMenu.gameObject.SetActive(false);
        _rewardManager.ClearAllGold(); // Удаляем все золото
        _gridManager.ClearGrid(); // Очищаем сетку
        StartGame(); // Заново запускаем игру
    }

    private void HandleGameWon(int collectedGold)
    {
        Debug.Log("Game Won!");
        _winMenu.Setup(collectedGold); // Показываем меню выигрыша
    }

    private void CenterCamera(Vector2 gridCenter)
    {
        //float cameraOffset = 2f; // Смещение камеры вниз
        Camera.main.transform.position = new Vector3(gridCenter.x, gridCenter.y /*- cameraOffset*/, -10);
    }

    private void PlaceBag()
    {
        float bagOffset = 1f;
        Vector2 gridCenter = _gridManager.GetGridCenter();
        Vector2 bagPosition = new Vector2(gridCenter.x, gridCenter.y - _gameConfig.fieldSize / 2f - bagOffset);

        // Создаем Bag, если он ещё не был создан
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


}
