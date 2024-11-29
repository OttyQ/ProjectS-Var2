using System;

public class ResourcePresenter : IResourceHandler
{
    private ResourceModel _resourceModel;
    private ResourceView _resourceView;

    public event Action<int> OnGameWon;

    public ResourcePresenter(ResourceModel resourceModel, ResourceView resourceView)
    {
        _resourceModel = resourceModel;
        _resourceView = resourceView;
    }

    public void UseShovel()
    {
        Console.WriteLine("ResourcePresenter UseShovel invoke!");
        _resourceModel.DecreaseShovelCount();
        _resourceView.UpdateShovelView(_resourceModel.ShovelCount);
    }

    public void AddGold()
    {
        _resourceModel.IncreaseCollectedGold();
        _resourceView.UpdateGoldView(_resourceModel.CollectedGold, _resourceModel.RequiredGold);

        //условие победы
        if (_resourceModel.CollectedGold == _resourceModel.RequiredGold)
        {
            OnGameWon?.Invoke(_resourceModel.CollectedGold);
        }
    }

    public bool CanDig()
    {
        return _resourceModel.ShovelCount > 0;
    }

   
}

