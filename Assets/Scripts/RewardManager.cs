using System.Collections.Generic;
using UnityEngine;

public class RewardManager : IRewardManager
{
    private float _spawnChance;
    private float _defaultSpawnChance;
    private float _chanceIncrement;
    private GoldSpawner _goldSpawner;
    private IResourceHandler _resourceHandler;

    // ������ ��� ������������ ���� ��������� �����������
    private List<RewardItemPresenter> _activePresenters = new List<RewardItemPresenter>();

    public RewardManager(float initialSpawnChance, float incrementChance, GameObject goldPrefab, IResourceHandler resourceHandler)
    {
        _defaultSpawnChance = initialSpawnChance;
        _spawnChance = _defaultSpawnChance;
        _chanceIncrement = incrementChance;
        _goldSpawner = new GoldSpawner(goldPrefab);
        _resourceHandler = resourceHandler;
    }

    public void RewardManagerProgression(float spawnChance, float incrementChance)
    {
        _defaultSpawnChance = spawnChance;
        _chanceIncrement = incrementChance;
    }
    public void TrySpawnGold(ICellModel cellModel, Transform cellTransform)
    {
        if (IsGoldSpawned())
        {
            Debug.Log("Gold spawn start!");
            SpawnGold(cellModel, cellTransform);
            ResetSpawnChance();
        }
        else
        {
            IncreaseSpawnChance();
        }
    }

    // �������������� ����� ������
    public void ForceSpawnGold(ICellModel cellModel, Transform cellTransform)
    {
        Debug.Log("Force spawning gold!");
        SpawnGold(cellModel, cellTransform);
    }

    private void SpawnGold(ICellModel cellModel, Transform cellTransform)
    {
        GameObject goldObject = _goldSpawner.SpawnGoldObject(cellTransform);
        RewardItemModel goldModel = new RewardItemModel();
        RewardItemView goldView = goldObject.GetComponent<RewardItemView>();  

        var presenter = new RewardItemPresenter(goldModel, goldView, cellModel, this, _resourceHandler);
        _activePresenters.Add(presenter);

        cellModel.GoldSpawned(); 
        Debug.Log("Gold spawn completed!");
    }

    public void ClearAllGold()
    {
        foreach (var presenter in _activePresenters)
        {
            var view = presenter.GetRewardItemView();
            if (view != null)
            {
                Object.Destroy(view.gameObject);
            }
            presenter.Dispose();
        }
        _activePresenters.Clear();
    }

    public void RemovePresenter(RewardItemPresenter presenter)
    {
        if (_activePresenters.Contains(presenter))
        {
            _activePresenters.Remove(presenter);
            presenter.Dispose();
        }
    }

    private bool IsGoldSpawned()
    {
        return Random.Range(0f, 1f) <= _spawnChance;
    }

    private void IncreaseSpawnChance()
    {
        _spawnChance += _chanceIncrement;
        Debug.Log($"Spawn chance increased to: {_spawnChance:F2}");
    }

    private void ResetSpawnChance()
    {
        _spawnChance = _defaultSpawnChance;
        Debug.Log($"Spawn chance reset to default: {_defaultSpawnChance:F2}");
    }
}
