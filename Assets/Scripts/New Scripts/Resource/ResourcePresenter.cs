using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePresenter
{
    private ResourceModel _resourceModel;
    private ResourceView _resourceView;

    public ResourcePresenter(ResourceModel resourceModel, ResourceView resourceView)
    {
        _resourceModel = resourceModel;
        _resourceView = resourceView;
    }   

    public void shovelUse()
    {
        _resourceModel.DecreaseShovelCount(); //��������� ���-�� �����

        _resourceView.UpdateShovelView(_resourceModel.GetShovelCount()); //��������� ShovelView �������� ������ �������� �����
    }

    public bool CanDig()
    {
        return _resourceModel.GetShovelCount() > 0;
    }
}
