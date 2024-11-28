using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItemPresenter
{
    private RewardItemModel _goldModel;
    private RewardItemView _goldView;
    private Vector3 _originalPosition; // �������� ������� ������
    private CellModel _parentCell;

    public RewardItemPresenter(RewardItemModel goldModel, RewardItemView goldView, CellModel parentCell)
    {
        _goldModel = goldModel;
        _goldView = goldView;
        _parentCell = parentCell;

        _originalPosition = _goldView.transform.position;
        

        BindEvents();
    }

    private void BindEvents()
    {
        _goldView.OnDragStart += HandleDragStart;
        _goldView.OnDragCont += HandleDrag;
        _goldView.OnDragEnd += HandleDragEnd;
    }

    private void UnbindEvents()
    {
        _goldView.OnDragStart -= HandleDragStart;
        _goldView.OnDragCont -= HandleDrag;
        _goldView.OnDragEnd -= HandleDragEnd;
    }

    private void HandleDragStart()
    {
        Debug.Log("Gold drag started.");
    }

    private void HandleDrag(Vector3 newPosition)
    {
        _goldView.transform.position = newPosition;
    }

    private void HandleDragEnd()
    {
        if (IsGoldInBag())
        {
            Debug.Log("Gold placed in Bag.");
            //_goldModel.Collect();    // �������� ������ ��� ���������
            _parentCell.GoldRemoved(); // ���������� ������
            Dispose();
            Object.Destroy(_goldView.gameObject); // ������� ������ ������
        }
        else
        {
            // ���������� ������ �� �������� �������
            _goldView.transform.position = _originalPosition;
        }
    }

    private bool IsGoldInBag()
    {
        // ���������, ����������� �� ������ � Bag
        Collider2D bagCollider = Physics2D.OverlapPoint(_goldView.transform.position, LayerMask.GetMask("Bag"));
        return bagCollider != null;
    }

    public void Dispose()
    {
        UnbindEvents();
    }
}
