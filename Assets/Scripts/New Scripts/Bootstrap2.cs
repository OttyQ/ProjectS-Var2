using System.Collections.Generic;
using UnityEngine;

public class Bootstrap2 : MonoBehaviour
{
    [SerializeField] private Config _gameConfig;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ResourceView _resourceView;

    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _goldPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;

    private ResourceModel _resModel;
    private ResourcePresenter _resPresenter;
    private RewardManager _rewardManager;

    void Start()
    {
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig is not assigned in Bootstrap2!");
            return;
        }

        StartGame();
    }

    private void StartGame()
    {
        // ������������� ��������
        ResourceInit();

        _rewardManager = new RewardManager(_gameConfig.goldSpawnChance, _gameConfig.goldSpawnChanceIncrement, _goldPrefab);
        // ������������� �����
        _gridManager.Initialize(_gameConfig.fieldSize, _cellPrefab, _boardPrefab, _gameConfig, _resPresenter, _rewardManager);
        _gridManager.GenerateGrid();
    }

    private void ResourceInit()
    {
        // �������������� ������������� ��������
        _resourceView.Init(_gameConfig.initialShovelCount, _gameConfig.requiredGoldBars);

        // �������������� ������ � ��������� �������� (���� ��� ��� �� �������)
        if (_resModel == null)
        {
            _resModel = new ResourceModel(_gameConfig.initialShovelCount, 0, _gameConfig.requiredGoldBars);
        }

        if (_resPresenter == null)
        {
            _resPresenter = new ResourcePresenter(_resModel, _resourceView);
        }

        // ���������� ������� � ���������� ���������
        _resModel.ResetModel(_gameConfig.requiredGoldBars, 0, _gameConfig.initialShovelCount);
    }

    public void RestartGame()
    {
        _gridManager.ClearGrid(); // ������� �����
        StartGame(); // ������ ��������� ����
    }
}
