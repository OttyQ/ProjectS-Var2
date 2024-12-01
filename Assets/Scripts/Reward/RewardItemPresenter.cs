using UnityEngine;

public class RewardItemPresenter
{
    private RewardItemModel _rewardItemModel;
    private RewardItemView _rewardItemView;
    private Vector3 _originalPosition;
    private ICellModel _parentCell;
    private IRewardManager _rewardManager;
    private IResourceHandler _resourcePresenter;

    public RewardItemPresenter(RewardItemModel rewardItemModel, RewardItemView rewardItemView, ICellModel parentCell, IRewardManager rewardManager, IResourceHandler resourcePresenter)
    {
        _rewardItemModel = rewardItemModel;
        _rewardItemView = rewardItemView;
        _parentCell = parentCell;
        _rewardManager = rewardManager;
        _resourcePresenter = resourcePresenter;

        _originalPosition = _rewardItemView.transform.position;
        BindEvents();
    }

    private void BindEvents()
    {
        _rewardItemView.OnDragStart += HandleDragStart;
        _rewardItemView.OnDragCont += HandleDrag;
        _rewardItemView.OnDragEnd += HandleDragEnd;
    }

    private void UnbindEvents()
    {
        _rewardItemView.OnDragStart -= HandleDragStart;
        _rewardItemView.OnDragCont -= HandleDrag;
        _rewardItemView.OnDragEnd -= HandleDragEnd;
    }

    private void HandleDragStart()
    {
        Debug.Log("Gold drag started.");
    }

    private void HandleDrag(Vector3 newPosition)
    {
        _rewardItemView.transform.position = newPosition;
    }

    private void HandleDragEnd()
    {
        if (IsGoldInBag())
        {
            Debug.Log("Gold placed in Bag.");
            _parentCell.GoldRemoved(); // Уведомляем клетку
            Dispose();

            _rewardManager.RemovePresenter(this); // Уведомляем RewardManager об удалении
            _resourcePresenter.AddGold(); // Добавляем золото

            Object.Destroy(_rewardItemView.gameObject);
        }
        else
        {
            // Возвращаем золото на исходную позицию
            _rewardItemView.transform.position = _originalPosition;
        }
    }

    private bool IsGoldInBag()
    {
        // Проверяем, пересеклось ли золото с Bag
        Collider2D bagCollider = Physics2D.OverlapPoint(_rewardItemView.transform.position, LayerMask.GetMask("Bag"));
        return bagCollider != null;
    }

    public void Dispose()
    {
        UnbindEvents();
    }

    public RewardItemView GetRewardItemView()
    {
        return _rewardItemView;
    }
}
