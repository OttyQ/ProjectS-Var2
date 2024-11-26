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
            //���������� ���-�� ����� � ResourceModel ����� ResourcePresenter
            _resourcePresenter.shovelUse();

            //����������� 
            _cellModel.IncreaseCurrentDepth();//��������� ������� � ������
            _cellView.UpdateDepth(_cellModel.GetCurrentDepth(), _cellModel.GetMaxDepth());//��������� view �������� ����� �������

            //����� ������
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
