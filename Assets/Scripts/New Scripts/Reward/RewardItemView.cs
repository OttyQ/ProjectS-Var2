using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItemView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action OnBeginDragEvent;
    public event Action OnDragEvent;
    public event Action<bool> OnEndDragEvent; // True, если сброшено в сумку

    [SerializeField] private Canvas _mainCanvas;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _originalPosition;
    private Transform _originalParent;
    private Transform _bagTransform;

    public void Init(Canvas mainCanvas, Transform bagTransform)
    {
        _mainCanvas = mainCanvas;
        _bagTransform = bagTransform;

        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPosition = _rectTransform.localPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = _rectTransform.parent;
        OnBeginDragEvent?.Invoke();

        _rectTransform.SetParent(_mainCanvas.transform, true);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        OnDragEvent?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool droppedInBag = CheckIfDroppedInBag();
        OnEndDragEvent?.Invoke(droppedInBag);
    }

    public void ResetPosition()
    {
        _rectTransform.SetParent(_originalParent, true);
        _rectTransform.localPosition = _originalPosition;
        _canvasGroup.blocksRaycasts = true;
    }

    private bool CheckIfDroppedInBag()
    {
        return _rectTransform.parent == _bagTransform;
    }
}
