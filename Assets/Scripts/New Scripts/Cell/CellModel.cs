using System;

public class CellModel
{
    private int _maxDepth;
    private int _currentDepth;
    private bool _hasGold;

    public event Action OnGoldSpawned;
    public event Action OnGoldRemoved;

    public CellModel(int curDepth, int maxDepth, bool hasGold)
    {
        _currentDepth = curDepth;
        _maxDepth = maxDepth;
        _hasGold = hasGold;
    }

    public void IncreaseCurrentDepth()
    {
        if (_currentDepth < _maxDepth && !_hasGold)
        {
            _currentDepth++;
        }
    }

    public int CurrentDepth => _currentDepth;
    public int MaxDepth => _maxDepth;
    public bool HasGold => _hasGold;

    public void GoldRemoved()
    {
        if (_hasGold)
        {
            _hasGold = false;
            OnGoldRemoved?.Invoke(); // Уведомляем подписчиков
        }
    }

    public void GoldSpawned()
    {
        if (!_hasGold)
        {
            _hasGold = true;
            OnGoldSpawned?.Invoke(); // Уведомляем подписчиков
        }
    }
}
