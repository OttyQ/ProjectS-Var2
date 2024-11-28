using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceModel
{
    private int _requiredGold;
    private int _collectedGold;
    private int _shovelCount;

    public ResourceModel(int requiredGold, int collectedGold, int shovelCount)
    {
        _requiredGold = requiredGold;
        _collectedGold = collectedGold;
        _shovelCount = shovelCount;
    }

    public int ShovelCount => _shovelCount;
    public int RequiredGold => _requiredGold;
    public int CollectedGold => _collectedGold;

    public void IncreaseCollectedGold()
    {
        _collectedGold++;
    }

    public void DecreaseShovelCount()
    {
        Console.WriteLine("ResourceModel DecreaseShovelCount invoke!");

        if (_shovelCount > 0)
        {
            _shovelCount--;
        }
    }

    public void ResetModel(int requiredGold, int collectedGold, int shovelCount)
    {
        _requiredGold = requiredGold;
        _collectedGold = collectedGold;
        _shovelCount = shovelCount;
    }
}

