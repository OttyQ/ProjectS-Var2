using UnityEngine;

public class RewardManager
{
    private float _spawnChance;
    private float _defaultSpawnChance;
    private float _chanceIncrement;
    private GoldSpawner _goldSpawner;

    public RewardManager(float initialSpawnChance, float incrementChance, GameObject goldPrefab)
    {
        _defaultSpawnChance = initialSpawnChance;
        _spawnChance = _defaultSpawnChance;
        _chanceIncrement = incrementChance;
        _goldSpawner = new GoldSpawner(goldPrefab);
    }

    // Метод для спавна золота
    public void TrySpawnGold(CellModel cellModel, Transform cellTransform)
    {
        if (IsGoldSpawned())
        {
            Debug.Log("Gold spawn start!");
            GameObject goldObject = _goldSpawner.SpawnGoldObject(cellTransform);
            RewardItemModel goldModel = new RewardItemModel();  // Создаём модель золота
            RewardItemView goldView = goldObject.GetComponent<RewardItemView>();  // Получаем View золота
            new RewardItemPresenter(goldModel, goldView, cellModel);  // Создаём Presenter золота

            cellModel.GoldSpawned();  // Связываем золото с клеткой
            Debug.Log("Gold spawn end!");
        }
        else
        {
            IncreaseSpawnChance();
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