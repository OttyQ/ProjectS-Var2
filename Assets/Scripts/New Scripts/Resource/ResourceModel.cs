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

    public int GetShovelCount()
    {
        return _shovelCount;
    }

    public int GeCollectedGold() {
        return _collectedGold;
    }

    public void IncreaseCollectedGold()
    {
        _collectedGold++;
    }

    public void DecreaseShovelCount()
    {
        _shovelCount--;
    }




}
