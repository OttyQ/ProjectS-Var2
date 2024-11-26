using System;
using System.Diagnostics;
using UnityEngine;

public class CellModel
{
    private int _maxDepth;
    private int _currentDepth;
    private bool _hasGold;

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

    public int GetCurrentDepth()
    {
        return _currentDepth;
    }

    public int GetMaxDepth()
    {
        return _maxDepth;
    }

    public bool GetHasGold()
    {
        return _hasGold;
    }

    private void GoldRemoved()
    {
        _hasGold = false;      
    }

    private void GoldSpawned()
    {
        _hasGold = true;
    }
}
