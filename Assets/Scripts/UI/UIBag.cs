using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Представляет игровую сумку (мешок) для сбора золотых наград.
/// Реализует функционал обработки перемещения объектов />.
/// </summary>
public class UIBag : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// Событие, вызываемое при добавлении золота в сумку.
    /// Подписчик (countHandler) использует его для обновления количества собранных наград.
    /// </summary>
    public event Action OnGoldAddedToBag;

    /// <summary>
    /// Вызывается, когда объект перетаскивается и отпускается над сумкой.
    /// Проверяет, является ли отпущенный объект золотом, 
    /// перемещает его в сумку и вызывает событие добавления золота.
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Bag OnDrop activate");
        var droppedItem = eventData.pointerDrag.GetComponent<RewardItem>();
        if(droppedItem != null)
        {
            droppedItem.transform.SetParent(this.transform, true);
            OnGoldAddedToBag?.Invoke();
            Debug.Log("Gold added to bag");
        }
        
    }
}
