using System.Collections.Generic;
using UnityEngine;

public class RewardManager: IRewardManager
{
    private float _spawnChance;
    private float _defaultSpawnChance;
    private float _chanceIncrement;
    private GoldSpawner _goldSpawner;
    private IResourceHandler _resourceHandler;

    // Список для отслеживания всех созданных презентеров
    private List<RewardItemPresenter> _activePresenters = new List<RewardItemPresenter>();
    public RewardManager(float initialSpawnChance, float incrementChance, GameObject goldPrefab, IResourceHandler resourceHandler)
    {
        _defaultSpawnChance = initialSpawnChance;
        _spawnChance = _defaultSpawnChance;
        _chanceIncrement = incrementChance;
        _goldSpawner = new GoldSpawner(goldPrefab);
        _resourceHandler = resourceHandler;
    }

    // Метод для спавна золота
    public void TrySpawnGold(ICellModel cellModel, Transform cellTransform)
    {
        if (IsGoldSpawned())
        {
            Debug.Log("Gold spawn start!");
            GameObject goldObject = _goldSpawner.SpawnGoldObject(cellTransform);
            RewardItemModel goldModel = new RewardItemModel();  // Создаём модель золота
            RewardItemView goldView = goldObject.GetComponent<RewardItemView>();  // Получаем View золота


            var presenter = new RewardItemPresenter(goldModel, goldView, cellModel, this, _resourceHandler); // Создаём Presenter золота

            _activePresenters.Add(presenter); // Добавляем презентер в список активных  // Создаём Presenter золота

            cellModel.GoldSpawned();  // Связываем золото с клеткой
            ResetSpawnChance();
            Debug.Log("Gold spawn end!");
        }
        else
        {
            IncreaseSpawnChance();
        }
    }

    public void ForceSpawnGold(ICellModel cellModel, Transform cellTransform)
    {
        Debug.Log("Force spawning gold!");
        GameObject goldObject = _goldSpawner.SpawnGoldObject(cellTransform); // Спавним объект золота
        RewardItemModel goldModel = new RewardItemModel();  // Создаём модель золота
        RewardItemView goldView = goldObject.GetComponent<RewardItemView>();  // Получаем View золота

        // Создаём Presenter золота
        var presenter = new RewardItemPresenter(goldModel, goldView, cellModel, this, _resourceHandler);
        _activePresenters.Add(presenter); // Добавляем презентер в список

        cellModel.GoldSpawned(); // Уведомляем модель клетки, что золото заспавнено
        Debug.Log("Gold force-spawned successfully!");
    }

    public void ClearAllGold()
    {
        foreach (var presenter in _activePresenters)
        {
            var view = presenter.GetRewItemView();
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