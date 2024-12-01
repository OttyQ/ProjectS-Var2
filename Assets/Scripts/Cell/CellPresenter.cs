using UnityEngine;

public class CellPresenter
{
    private CellModel _cellModel;
    private CellView _cellView;
    private IResourceHandler _resourceHandler;
    private IRewardManager _rewardManager;

    public CellPresenter(CellModel cellModel, CellView cellView, IResourceHandler resourceHandler, IRewardManager rewardManager)
    {
        _cellModel = cellModel;
        _cellView = cellView;
        _resourceHandler = resourceHandler;
        _rewardManager = rewardManager;

        SubscribeEvents();
    }

    public void Dispose()
    {
        UnsubscribeEvents();
        _cellView.ResetEvents(); // —брасываем событи€
        _cellModel = null;
        _cellView = null;
        _resourceHandler = null;
        _rewardManager = null;
    }

    private void SubscribeEvents()
    {
        _cellView.OnCellClicked += OnCellDigged;
    }

    private void UnsubscribeEvents()
    {
        _cellView.OnCellClicked -= OnCellDigged;
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

    private bool CanDig()
    {
        return _cellModel.CurrentDepth < _cellModel.MaxDepth &&
               !_cellModel.HasGold &&
               _resourceHandler.CanDig();
    }
}
