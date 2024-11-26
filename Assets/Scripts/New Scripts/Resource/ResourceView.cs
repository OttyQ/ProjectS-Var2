using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI shovelText;

    public void Init(int startShovelCount, int requiredGold, int collectedGold = 0)
    {
        shovelText.text = startShovelCount.ToString();
        goldText.text = $"{collectedGold}/{requiredGold}";
    }

    public void UpdateShovelView(int remainingShovelCount)
    {
        shovelText.text = remainingShovelCount.ToString();
    }

    public void UpdateGoldView(int collectedGold, int requiredGold)
    {
        goldText.text = $"{collectedGold}/{requiredGold}";
    }
}
