using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI shovelText;

    public void Init(int startShovelCount, int requiredGold, int collectedGold = 0)
    {
        if (shovelText == null || goldText == null)
        {
            Debug.LogError("TextMeshProUGUI references are not assigned!");
            return;
        }

        shovelText.text = startShovelCount.ToString();
        goldText.text = $"{collectedGold}/{requiredGold}";
    }

    public void UpdateShovelView(int remainingShovelCount)
    {
        if (shovelText == null)
        {
            Debug.LogError("shovelText reference is not assigned!");
            return;
        }

        shovelText.text = remainingShovelCount.ToString();
    }

    public void UpdateGoldView(int collectedGold, int requiredGold)
    {
        if (goldText == null)
        {
            Debug.LogError("goldText reference is not assigned!");
            return;
        }

        goldText.text = $"{collectedGold}/{requiredGold}";
    }
}