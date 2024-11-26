using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItemPresenter
{
    private readonly RewardItemModel _model;
    private readonly RewardItemView _view;
    //private readonly IBagHandler _bagHandler;

    public RewardItemPresenter(RewardItemModel model, RewardItemView view /*IBagHandler bagHandler*/)
    {
        _model = model;
        _view = view;
        //_bagHandler = bagHandler;

        _view.OnBeginDragEvent += HandleBeginDrag;
        _view.OnDragEvent += HandleDrag;
        _view.OnEndDragEvent += HandleEndDrag;
    }

    private void HandleBeginDrag()
    {
        Debug.Log("RewardItemPresenter: Begin Drag.");
    }

    private void HandleDrag()
    {
        Debug.Log("RewardItemPresenter: Dragging.");
    }

    private void HandleEndDrag(bool droppedInBag)
    {
        if (droppedInBag)
        {
            _model.MoveToBag();
            //_bagHandler.AddToBag(_model); // Уведомить сумку, что золото добавлено
            GameObject.Destroy(_view.gameObject); // Удалить визуальный объект
        }
        else
        {
            _model.ResetPosition();
            _view.ResetPosition();
        }
    }

    public void Unsubscribe()
    {
        _view.OnBeginDragEvent -= HandleBeginDrag;
        _view.OnDragEvent -= HandleDrag;
        _view.OnEndDragEvent -= HandleEndDrag;
    }
}
