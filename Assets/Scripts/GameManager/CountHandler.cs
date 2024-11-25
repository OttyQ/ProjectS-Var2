using System;
using TMPro;
using UnityEngine;

/// <summary>
/// �������� �� ���������� �������� ����������, ������ ��� ���������� ���������� ����� � ��������� ������.
/// </summary>
public class CountHandler : MonoBehaviour
{
    /// <summary>
    /// �������, ���������� ��� ��������� ���������� ���������� �����.
    /// </summary>
    public event Action<int> OnShovelCountChanged;

    /// <summary>
    /// �������, ���������� ��� ��������� ���������� ��������� ������.
    /// �������� ������� ���������� ��������� ������ � ��������� ����������.
    /// </summary>
    public event Action<int, int> OnRewardCountChanged;

    /// <summary>
    /// �������, ���������� ��� ����� ���� ������.
    /// </summary>
    public event Action OnAllRewardCollected;                   

    private int _countShovels;                               
    private int _collectedRewards = 0;                           
    private int _requiredRewards;

    /// <summary>
    /// ������������� ���������.
    /// </summary>
    public void Initialize(int countShovels,int requiredRewards, int collectedRewards = 0)
    {
        _countShovels = countShovels;
        _requiredRewards = requiredRewards;
        _collectedRewards = collectedRewards;
    }

    /// <summary>
    /// ���������� UI �������� � �������� ����������.
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