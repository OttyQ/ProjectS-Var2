using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellPresenter
{
    private CellModel _cellModel;
    private CellView _cellView;
    private IResourceHandler _resourceHandler;

    public CellPresenter(CellModel cellModel, CellView cellView, IResourceHandler resourceHandler)
    {
        _cellModel = cellModel;
        _cellView = cellView;
        _resourceHandler = resourceHandler;

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

            // Спавн золота или другие действия
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
}

