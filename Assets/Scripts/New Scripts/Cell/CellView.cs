using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer cellRenderer;

    public event Action OnCellClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCellClicked?.Invoke();
        Debug.Log($"{gameObject.name} was Clicked ");
    }

    public void UpdateDepth(int currentDepth, int maxDepth)
    {
        float depthRatio = (float)currentDepth / maxDepth;
        cellRenderer.color = Color.Lerp(Color.white, Color.black, depthRatio);
    }

    public void ResetEvents()
    {
        OnCellClicked = null; // Полностью очищаем подписчиков
    }
}
