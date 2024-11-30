using UnityEngine;
public interface IRewardManager
{
    void TrySpawnGold(ICellModel cellModel, Transform cellTransform);
    void ForceSpawnGold(ICellModel cellModel, Transform cellTransform);
    void ClearAllGold();
    void RemovePresenter(RewardItemPresenter presenter);
}