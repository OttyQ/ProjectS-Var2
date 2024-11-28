using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellPresenter
{
    private CellModel _cellModel;
    private CellView _cellView;
    private IResourceHandler _resourceHandler;
    private RewardManager _rewardManager;

    public CellPresenter(CellModel cellModel, CellView cellView, IResourceHandler resourceHandler, RewardManager rewardManager)
    {
        _cellModel = cellModel;
        _cellView = cellView;
        _resourceHandler = resourceHandler;
        _rewardManager = rewardManager;

        Bind();
    }

    public void Bind()
    {
        _cellView.OnCellClicked += OnCellDigged;
    }

    private void OnCellDigged()
    {
        if (CanDig())
        {
            _resourceHandler.UseShovel();

            _cellModel.IncreaseCurrentDepth();
            _cellView.UpdateDepth(_cellModel.CurrentDepth, _cellModel.MaxDepth);

            Debug.Log("Try to spawn Gold!");
            _rewardManager.TrySpawnGold(_cellModel, _cellView.transform);
        }
        else
        {
            Debug.LogWarning("Cannot dig further!");
        }
    }

    public bool CanDig()
    {
        return (_cellModel.CurrentDepth < _cellModel.MaxDepth && !_cellModel.HasGold && _resourceHandler.CanDig());
    }

    public void UnBind()
    {
        if (_cellView != null)
        {
            _cellView.OnCellClicked -= OnCellDigged;
        }
    }

    public void Dispose()
    {
        UnBind();
        _cellView.ResetEvents(); // —брасываем событи€
        _cellModel = null;
        _cellView = null;
        _resourceHandler = null;
    }
}


