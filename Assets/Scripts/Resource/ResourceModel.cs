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
            Console.WriteLine($"Collected gold increased: {_collectedGold}/{_requiredGold}");
        }
        else
        {
            Console.WriteLine("Cannot increase collected gold: limit reached.");
        }
    }

    public void DecreaseShovelCount()
    {
        Console.WriteLine("ResourceModel DecreaseShovelCount invoked!");

        if (_shovelCount > 0)
        {
            _shovelCount--;
            Console.WriteLine($"Shovel count decreased: {_shovelCount}");
        }
        else
        {
            Console.WriteLine("Cannot decrease shovel count: no shovels left.");
        }
    }

    public void ResetModel(int requiredGold, int collectedGold, int shovelCount)
    {
        _requiredGold = requiredGold;
        _collectedGold = collectedGold;
        _shovelCount = shovelCount;
    }
}
