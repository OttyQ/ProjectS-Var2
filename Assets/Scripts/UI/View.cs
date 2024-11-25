using TMPro;
using UnityEngine;


/// <summary>
/// Класс, отвечающий за обновление UI-элементов на экране игры,
/// таких как счетчик собранных наград и оставшихся лопат.
/// </summary>
public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardCounterText;
    [SerializeField] private TextMeshProUGUI shovelCounterText;

    public void UpdateRewardCount(int rewardsCollected, int requiredRewards)
    {
        if(rewardCounterText != null)
        {
            rewardCounterText.text = $"{rewardsCollected}/{requiredRewards}";
        }
        else
        {
            Debug.LogWarning("RewardCounterText is not assigned in the inspector!");
        }
    }
    public void UpdateShovelCount(int shovelsRemaining)
    {
        if (shovelCounterText != null)
        {
            shovelCounterText.text = shovelsRemaining.ToString();
        }
        else
        {
            Debug.LogWarning("ShovelCounterText is not assigned in the inspector!");
        }
    }
 
}
