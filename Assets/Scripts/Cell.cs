using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ����� ��� ���������� �������, � ������������ ������.
/// </summary>
public class Cell : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// �������, ������� �����������, ��� ������� ������.
    /// </summary>
    public event Func<Transform, bool> GoldDigged;

    /// <summary>
    /// �������, ������� �����������, ����� ������ ���� ������.
    /// </summary>
    public event Action OnCellDigged;

    private int _maxDepth;
    private int _currentDepth;
    private bool _hasGold;
    
    private RewardItem rewardItemOnCell;
    private CellRenderer cellRenderer;
    private CountHandler countHandler;

    /// <summary>
    /// ������������� ������ � ��������� �����������.
    /// </summary>
    /// <param name="maxDepth">������������ ������� ��� �����.</param>
    /// <param name="currentDepth">������� ������� �����.</param>
    /// <param name="countHandler">���������� ����� ��� ������������ ���������� �����.</param>
    /// <param name="hasGold">����, �����������, ���� �� ������ � ������.</param>
    public void Initialize(int maxDepth, int currentDepth, CountHandler countHandler, bool hasGold)
    {
        _maxDepth = maxDepth;
        _currentDepth = currentDepth;
        _hasGold = hasGold;
        this.countHandler = countHandler;

        cellRenderer = GetComponent<CellRenderer>();
        cellRenderer?.Initialize(maxDepth, currentDepth);
    }

    private void Start()
    {
        cellRenderer?.UpdateColor(_currentDepth);
    }

    private void OnDisable()
    {
        UnsubscribeFromRewardEvents();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Dig();
    }

    private void Dig()
    {
        if (!CanDig()) return;

        _currentDepth--;
        Debug.Log($"Current depth: {_currentDepth}");
        cellRenderer?.UpdateColor(_currentDepth); //���������� ����������� ����������� ������.

        if (GoldDigged?.Invoke(transform) == true)
        {
            AssignGold();
        }

        OnCellDigged?.Invoke();
    }

    /// <summary>
    /// ���������, ����� �� ������ � ���� ������.
    /// </summary>
    private bool CanDig()
    {
        return !_hasGold && _currentDepth > 0 && countHandler.GetRemainingShovels() > 0;
    }

    /// <summary>
    /// ��������� ������ ��� ���� ������, ���� ��� ����.
    /// </summary>
    public void AssignGold()
    {
        rewardItemOnCell = GetComponentInChildren<RewardItem>();
        if (rewardItemOnCell != null)
        {
            rewardItemOnCell.onGoldSpawned += GoldSpawned;
            rewardItemOnCell.onGoldRemoved += GoldRemoved;
        }
    }

    private void UnsubscribeFromRewardEvents()
    {
        if (rewardItemOnCell != null)
        {
            rewardItemOnCell.onGoldSpawned -= GoldSpawned;
            rewardItemOnCell.onGoldRemoved -= GoldRemoved;
        }
    }

    private void GoldRemoved()
    {
        _hasGold = false;
        Debug.Log("Gold was removed from the cell!");
    }

    private void GoldSpawned()
    {
        _hasGold = true;
        Debug.Log("Gold appeared on the cell!");
    }

    public bool HasGold() => _hasGold;

    public int GetDepth() => _currentDepth;
}
