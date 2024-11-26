using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePresenter : IResourceHandler
{
    private ResourceModel _resourceModel;
    private ResourceView _resourceView;

    public ResourcePresenter(ResourceModel resourceModel, ResourceView resourceView)
    {
        _resourceModel = resourceModel;
        _resourceView = resourceView;
    }

    public void UseShovel()
    {
        _resourceModel.DecreaseShovelCount();
        _resourceView.UpdateShovelView(_resourceModel.ShovelCount);
    }

    public void AddGold()
    {
        _resourceModel.IncreaseCollectedGold();
        _resourceView.UpdateGoldView(_resourceModel.CollectedGold, _resourceModel.RequiredGold);
    }

    public bool CanDig()
    {
        return _resourceModel.ShovelCount > 0;
    }

   
}

