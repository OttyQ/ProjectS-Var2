using System;

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
        if (_collectedGold < _requiredGold)
        {
            _collectedGold++;
        }
    }

    public void DecreaseShovelCount()
    {
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
