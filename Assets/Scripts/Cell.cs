using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Класс для управления клеткой, с возможностью копать.
/// </summary>
public class Cell : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Событие, которое срабатывает, при выкопке золото.
    /// </summary>
    public event Func<Transform, bool> GoldDigged;

    /// <summary>
    /// Событие, которое срабатывает, когда клетка была вырыта.
    /// </summary>
    public event Action OnCellDigged;

    private int _maxDepth;
    private int _currentDepth;
    private bool _hasGold;
    
    private RewardItem rewardItemOnCell;
    private CellRenderer cellRenderer;
    private CountHandler countHandler;

    /// <summary>
    /// Инициализация клетки с заданными параметрами.
    /// </summary>
    /// <param name="maxDepth">Максимальная глубина для копки.</param>
    /// <param name="currentDepth">Текущая глубина копки.</param>
    /// <param name="countHandler">Обработчик счета для отслеживания оставшихся лопат.</param>
    /// <param name="hasGold">Флаг, указывающий, есть ли золото в клетке.</param>
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
        cellRenderer?.UpdateColor(_currentDepth); //Обновление визуального отображения клетки.

        if (GoldDigged?.Invoke(transform) == true)
        {
            AssignGold();
        }

        OnCellDigged?.Invoke();
    }

    /// <summary>
    /// Проверяет, можно ли копать в этой клетке.
    /// </summary>
    private bool CanDig()
    {
        return !_hasGold && _currentDepth > 0 && countHandler.GetRemainingShovels() > 0;
    }

    /// <summary>
    /// Назначает золото для этой клетки, если оно есть.
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
