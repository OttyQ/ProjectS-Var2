using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardItemView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action OnDragStart;  // ������� ������ ��������������
    public event Action<Vector3> OnDragCont;  // ������� ��������������
    public event Action OnDragEnd;  // ������� ��������� ��������������

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnbegDrag invoke!");
        OnDragStart?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag invoke!");
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPosition.z = 0; // ������� �������� �� ��� Z
        OnDragCont?.Invoke(worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag invoke!");
        OnDragEnd?.Invoke();
    }

   
}
