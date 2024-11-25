using System;
using UnityEngine;


/// <summary>
///  ласс дл€ управлени€ логикой спауна золота в игре.
/// ќбрабатывает шанс по€влени€ золота в клетке и управл€ет его спауном.
/// </summary>
public class RewardManager : MonoBehaviour
{
    private float _spawnChance;
    private float _defaultSpawnChance;
    private float _chanceIncrement;

    private IGoldSpawner goldSpawner;

    public void Initialize(float initialSpawnChance, float incrementChance, GameObject goldPrefab)
    {
        if (goldPrefab == null)
            throw new ArgumentNullException(nameof(goldPrefab), "Gold prefab cannot be null");

        _defaultSpawnChance = initialSpawnChance;
        _spawnChance = _defaultSpawnChance;
        _chanceIncrement = incrementChance;

        goldSpawner = new GoldSpawner(goldPrefab);
    }

    private void OnDisable()
    {
        UnsubscribeFromCellEvents(FindObjectsOfType<Cell>());
    }

    /// <summary>
    /// ѕодписка на событи€ клеток дл€ попытки спавна золото при вскапывании клетки.
    /// </summary>
    /// <param name="cells">ћассив клеток</param>
    public void SubscribeToCellEvents(Cell[] cells)
    {
        foreach (var cell in cells)
        {
            cell.GoldDigged += TrySpawnGold;
        }

        Debug.Log("Subscribed to cell events from RewardManager!");
    }

    public void UnsubscribeFromCellEvents(Cell[] cells)
    {
        foreach (var cell in cells)
        {
            cell.GoldDigged -= TrySpawnGold;
        }
    }

    /// <summary>
    /// ѕытаетс€ заспавнить золото в клетке. ≈сли золото не по€вл€етс€, увеличивает шанс его по€влени€.
    /// </summary>
    private bool TrySpawnGold(Transform cell)
    {
        if (IsGoldSpawned())
        {
            goldSpawner.SpawnGoldObject(cell);
            ResetSpawnChance();
            return true;
        }

        IncreaseSpawnChance();
        return false;
    }

    private bool IsGoldSpawned()
    {
        return UnityEngine.Random.Range(0f, 1f) <= _spawnChance;
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

    /// <summary>
    /// –учной спавн золота в клетке. »спользуетс€ при загрузке игры.
    /// </summary>
    public void HandleGoldSpawn(Transform cell)
    {
        Debug.Log("Handle spawn gold!");
        goldSpawner.SpawnGoldObject(cell);
    }
}
