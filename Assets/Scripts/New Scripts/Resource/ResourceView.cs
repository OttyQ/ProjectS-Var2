using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI ShovelText;


    public void Init(int startShovelCount, int requiredGold, int collectedGold = 0)
    {
        ShovelText.text = startShovelCount.ToString();
        goldText.text = $"{collectedGold}/{requiredGold}";
    }

    public void UpdateShovelView(int remainingShovelCount)
    {
        ShovelText.text = remainingShovelCount.ToString();
    }

    public void UpdateGoldView(int collectedGold, int requiredGold)
    {
        goldText.text = $"{collectedGold}/{requiredGold}";
    }
}
