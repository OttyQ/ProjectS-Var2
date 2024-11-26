using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CellPresenter
{
    private CellModel _cellModel;
    private CellView _cellView;

    private ResourcePresenter _resourcePresenter;

    public CellPresenter(CellModel cellModel, CellView cellView, ResourcePresenter resourcePresenter)
    {
        _cellModel = cellModel;
        _cellView = cellView;
        _resourcePresenter = resourcePresenter;

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
            //уменьшение кол-ва лопат в ResourceModel через ResourcePresenter
            _resourcePresenter.shovelUse();

            //вскапывание 
            _cellModel.IncreaseCurrentDepth();//уменьшаем глубину у модели
            _cellView.UpdateDepth(_cellModel.GetCurrentDepth(), _cellModel.GetMaxDepth());//обновляем view согласно новой глубине

            //спавн золота
            //realisation
        }
    }

    public bool CanDig()
    {
        return (_cellModel.GetCurrentDepth() < _cellModel.GetMaxDepth() && !_cellModel.GetHasGold() && _resourcePresenter.CanDig());
    }

    public void UnBind()
    {
        if (_cellView != null)
        {
            _cellView.OnCellClicked -= OnCellDigged;
        }
    }
}
