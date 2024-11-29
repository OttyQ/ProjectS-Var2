using UnityEngine;

public class RewardItemPresenter
{
    private RewardItemModel _rewItemModel;
    private RewardItemView _rewItemView;
    private Vector3 _originalPosition; // Исходная позиция золота
    private ICellModel _parentCell;

    private IRewardManager _rewardManager;
    private IResourceHandler _resourcePresenter;

    public RewardItemPresenter(RewardItemModel rewItemModel, RewardItemView rewItemView, ICellModel parentCell, IRewardManager rewardManager, IResourceHandler resourcePresenter)
    {
        _rewItemModel = rewItemModel;
        _rewItemView = rewItemView;
        _parentCell = parentCell;
        _rewardManager = rewardManager;
        _resourcePresenter = resourcePresenter;

        _originalPosition = _rewItemView.transform.position;
        

        BindEvents();
    }

    private void BindEvents()
    {
        _rewItemView.OnDragStart += HandleDragStart;
        _rewItemView.OnDragCont += HandleDrag;
        _rewItemView.OnDragEnd += HandleDragEnd;
    }

    private void UnbindEvents()
    {
        _rewItemView.OnDragStart -= HandleDragStart;
        _rewItemView.OnDragCont -= HandleDrag;
        _rewItemView.OnDragEnd -= HandleDragEnd;
    }

    private void HandleDragStart()
    {
        Debug.Log("Gold drag started.");
    }

    private void HandleDrag(Vector3 newPosition)
    {
        _rewItemView.transform.position = newPosition;
    }

    private void HandleDragEnd()
    {
        if (IsGoldInBag())
        {
            Debug.Log("Gold placed in Bag.");
            _parentCell.GoldRemoved(); // Уведомляем клетку
            Dispose();

            _rewardManager.RemovePresenter(this); // Уведомляем RewardManager об удалении

            _resourcePresenter.AddGold();//добавляем золото

            Object.Destroy(_rewItemView.gameObject); // Удаляем объект золота
        }
        else
        {
            // Возвращаем золото на исходную позицию
            _rewItemView.transform.position = _originalPosition;
        }
    }

    private bool IsGoldInBag()
    {
        // Проверяем, пересеклось ли золото с Bag
        Collider2D bagCollider = Physics2D.OverlapPoint(_rewItemView.transform.position, LayerMask.GetMask("Bag"));
        return bagCollider != null;
    }

    public void Dispose()
    {
        UnbindEvents();
    }

    public RewardItemView GetRewItemView()
    {
        return _rewItemView;
    }
}
