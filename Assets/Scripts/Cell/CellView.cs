using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer _cellRenderer;

    public event Action OnCellClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCellClicked?.Invoke();
        Debug.Log($"Cell '{gameObject.name}' clicked at position {transform.position}");
    }

    public void UpdateDepth(int currentDepth, int maxDepth)
    {
        float depthRatio = (float)currentDepth / maxDepth;
        _cellRenderer.color = Color.Lerp(Color.white, Color.black, depthRatio);
    }

    public void ResetEvents()
    {
        OnCellClicked = null;
    }
}
