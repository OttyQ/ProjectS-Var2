using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ������������ ������� ����� (�����) ��� ����� ������� ������.
/// ��������� ���������� ��������� ����������� �������� />.
/// </summary>
public class UIBag : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// �������, ���������� ��� ���������� ������ � �����.
    /// ��������� (countHandler) ���������� ��� ��� ���������� ���������� ��������� ������.
    /// </summary>
    public event Action OnGoldAddedToBag;

    /// <summary>
    /// ����������, ����� ������ ��������������� � ����������� ��� ������.
    /// ���������, �������� �� ���������� ������ �������, 
    /// ���������� ��� � ����� � �������� ������� ���������� ������.
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
