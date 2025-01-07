using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Поля
    private int _size;
    private GameObject _cellPrefab;
    private SpriteRenderer _boardPrefab;
    private int _maxDepth;
    private IResourceHandler _resourcePresenter;
    private SpriteRenderer _board;
    private IRewardManager _rewardManager;

    private List<GameObject> _cells; //cписок представлений клеток
    private List<CellModel> _cellModels; //cписок моделей клеток
    private List<CellPresenter> _presenters; //cписок презентеров клеток

    // Методы
    public void Initialize(int size, GameObject cellPrefab, SpriteRenderer boardPrefab, int maxDepth, IResourceHandler resourcePresenter, IRewardManager rewardManager)
    {
        _size = size;
        _cellPrefab = cellPrefab;
        _boardPrefab = boardPrefab;
        _maxDepth = maxDepth;
        _resourcePresenter = resourcePresenter;
        _rewardManager = rewardManager;

        _cells = new List<GameObject>();
        _cellModels = new List<CellModel>();
        _presenters = new List<CellPresenter>();
    }

    public void GenerateGrid()
    {
        ClearGrid();
        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                CreateCell(x, y, new CellModel(0, _maxDepth, false));
            }
        }
        SetupGrid();
    }

    public void HandleGenerateGrid(List<CellData> cellsData, int saveMaxDepth)
    {
        ClearGrid();
        if (cellsData == null || cellsData.Count == 0)
        {
            Debug.LogWarning("Cell data is empty or null. Grid will be empty.");
            return;
        }

        for (int x = 0; x < _size; x++)
        {
            for (int y = 0; y < _size; y++)
            {
                int index = x * _size + y;
                if (index >= cellsData.Count) break;

                CellData cellData = cellsData[index];
                var cellModel = new CellModel(cellData.depth, saveMaxDepth, cellData.hasGold);
                var cellObject = CreateCell(x, y, cellModel);

                if (cellData.hasGold)
                {
                    _rewardManager.ForceSpawnGold(cellModel, cellObject.transform);
                }
            }
        }
        SetupGrid();
    }

    private GameObject CreateCell(int x, int y, CellModel cellModel)
    {
        _cellModels.Add(cellModel);

        var cellObject = Instantiate(_cellPrefab, new Vector2(x, y), Quaternion.identity);
        cellObject.name = $"Cell ({x},{y})";
        _cells.Add(cellObject);

        var cellView = cellObject.GetComponent<CellView>();
        var cellPresenter = new CellPresenter(cellModel, cellView, _resourcePresenter, _rewardManager);
        _presenters.Add(cellPresenter);

        cellView.UpdateDepth(cellModel.CurrentDepth, cellModel.MaxDepth);

        return cellObject;
    }

    public void ClearGrid()
    {
        foreach (var presenter in _presenters)
        {
            presenter.Dispose();
        }

        foreach (var cell in _cells)
        {
            Destroy(cell);
        }

        if (_board != null)
        {
            Destroy(_board.gameObject);
            _board = null;
        }

        _presenters.Clear();
        _cells.Clear();
        _cellModels.Clear();
    }

    public List<CellData> GetCellData()
    {
        var cellsData = new List<CellData>();
        foreach (var cellModel in _cellModels)
        {
            cellsData.Add(new CellData(cellModel.CurrentDepth, cellModel.HasGold));
        }
        return cellsData;
    }

    public Vector2 GetGridCenter()
    {
        return new Vector2((float)_size / 2 - 0.5f, (float)_size / 2 - 0.5f);
    }

    private void SetupGrid()
    {
        var center = GetGridCenter();
        SetBorder(center);
    }

    private void SetBorder(Vector2 center)
    {
        _board = Instantiate(_boardPrefab, center, Quaternion.identity);
        _board.size = new Vector2(_size, _size);
    }
}
