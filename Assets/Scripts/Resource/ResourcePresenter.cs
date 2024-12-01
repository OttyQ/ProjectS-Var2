using System;
using UnityEngine;

public class ResourcePresenter : IResourceHandler
{
    private readonly ResourceModel _resourceModel;
    private readonly ResourceView _resourceView;

    public event Action<int> OnGameWon;

    public ResourcePresenter(ResourceModel resourceModel, ResourceView resourceView)
    {
        _resourceModel = resourceModel;
        _resourceView = resourceView;
    }

    public void UseShovel()
    {
        Debug.Log("ResourcePresenter: UseShovel invoked.");
        if (_resourceModel.ShovelCount > 0)
        {
            _resourceModel.DecreaseShovelCount();
            _resourceView.UpdateShovelView(_resourceModel.ShovelCount);
        }
        else
        {
            Debug.LogWarning("ResourcePresenter: No shovels left.");
        }
    }

    public void AddGold()
    {
        _resourceModel.IncreaseCollectedGold();
        _resourceView.UpdateGoldView(_resourceModel.CollectedGold, _resourceModel.RequiredGold);

        if (_resourceModel.CollectedGold == _resourceModel.RequiredGold)
        {
            Debug.Log("ResourcePresenter: Victory condition met.");
            OnGameWon?.Invoke(_resourceModel.CollectedGold);
        }
    }

    public bool CanDig()
    {
        return _resourceModel.ShovelCount > 0;
    }
}
