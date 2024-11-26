using System;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Класс для управления взаимодействием с предметом награды (золота), который можно перетаскивать.
/// </summary>
public class RewardItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action onGoldSpawned;
    public event Action onGoldRemoved;

    private Canvas _mainCanvas;
    private RectTransform _rectTransform;
    private Transform _originalParent;
    private Vector3 _originalPosition;
    private CanvasGroup _canvasGroup;
    private UIBag _uiBag;

    private void Start()
    {
        _mainCanvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _uiBag = FindObjectOfType<UIBag>();
        onGoldSpawned?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = _rectTransform.parent;
        _originalPosition = _rectTransform.localPosition;

        _rectTransform.SetParent(_mainCanvas.transform, true); 
        _canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsDroppedInBag())
        {
            HandleGoldDroppedInBag();
        }
        else
        {
            ResetPosition();
        }
    }

    private bool IsDroppedInBag()
    {
        return _rectTransform.parent == _uiBag.transform;
    }

    /// <summary>
    /// Обрабатывает ситуацию, когда золото было сброшено в сумку.
    /// </summary>
    private void HandleGoldDroppedInBag()
    {
        Debug.Log("RewItem: Gold moved to the bag.");
        onGoldRemoved?.Invoke();
        Destroy(gameObject);
    }

    /// <summary>
    /// Восстанавливает исходное положение золота, если он не был сброшен в сумку.
    /// </summary>
    private void ResetPosition()
    {
        Debug.Log("RewItem: Gold returned to the cell.");
        _rectTransform.SetParent(_originalParent, true);
        _rectTransform.localPosition = _originalPosition;
        _canvasGroup.blocksRaycasts = true;
    }
}
