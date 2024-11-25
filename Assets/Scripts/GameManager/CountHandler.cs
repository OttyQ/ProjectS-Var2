using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Отвечает за управление игровыми счетчиками, такими как количество оставшихся лопат и собранных наград.
/// </summary>
public class CountHandler : MonoBehaviour
{
    /// <summary>
    /// Событие, вызываемое при изменении количества оставшихся лопат.
    /// </summary>
    public event Action<int> OnShovelCountChanged;

    /// <summary>
    /// Событие, вызываемое при изменении количества собранных наград.
    /// Передает текущее количество собранных наград и требуемое количество.
    /// </summary>
    public event Action<int, int> OnRewardCountChanged;

    /// <summary>
    /// Событие, вызываемое при сборе всех наград.
    /// </summary>
    public event Action OnAllRewardCollected;                   

    private int _countShovels;                               
    private int _collectedRewards = 0;                           
    private int _requiredRewards;

    /// <summary>
    /// Инициализация счетчиков.
    /// </summary>
    public void Initialize(int countShovels,int requiredRewards, int collectedRewards = 0)
    {
        _countShovels = countShovels;
        _requiredRewards = requiredRewards;
        _collectedRewards = collectedRewards;
    }

    /// <summary>
    /// Обновление UI элемнтов с текущими значениями.
    /// </summary>
    public void UpdateView()
    {
        OnShovelCountChanged?.Invoke(_countShovels);
        OnRewardCountChanged?.Invoke(_collectedRewards, _requiredRewards);
    }

    public void UseShovel()
    {
        if (_countShovels > 0)
        {
            _countShovels--;
            OnShovelCountChanged?.Invoke(_countShovels);

            if (_countShovels == 0)
            {
                Debug.Log("We're out of shovels.");
            }
        }
    }

    public void CollectReward()
    {
        if (_collectedRewards < _requiredRewards)
        {
            _collectedRewards++;
            OnRewardCountChanged?.Invoke(_collectedRewards, _requiredRewards);

            if (_collectedRewards == _requiredRewards)
            {
                Debug.Log("All awards are collected!");
                OnAllRewardCollected?.Invoke();
            }
        }
    }

    public int GetRemainingShovels()
    {
        return _countShovels;
    }

    public int GetCollectedRewards()
    {
        return _collectedRewards;
    }

    public int GetRequiredRewards()
    {
        return _requiredRewards;
    }
}