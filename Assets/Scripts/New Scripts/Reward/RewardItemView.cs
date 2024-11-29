using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItemView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action OnDragStart;  // Событие начала перетаскивания
    public event Action<Vector3> OnDragCont;  // Событие перетаскивания
    public event Action OnDragEnd;  // Событие окончания перетаскивания

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnbegDrag invoke!");
        OnDragStart?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag invoke!");
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPosition.z = -1; // Убираем смещение по оси Z
        OnDragCont?.Invoke(worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag invoke!");
        OnDragEnd?.Invoke();
    }

   
}
