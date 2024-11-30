using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int _size;
    private GameObject _cellPrefab;
    private SpriteRenderer _boardPrefab;
    private Config _gameConfig;
    private IResourceHandler _resPresenter;
    private SpriteRenderer _board;
    private IRewardManager _rewardManager;

    private List<GameObject> _cells; // ������ ���� ��������� ������ (�������������)
    private List<CellModel> _cellModels; // ������ ������ ������� ������
    private List<CellPresenter> _presenters;

    public void Initialize(int size, GameObject cellPrefab, SpriteRenderer boardPrefab, Config gameConfig, IResourceHandler resourcePresenter, IRewardManager rewardManager)
    {
        _size = size;
        _cellPrefab = cellPrefab;
        _boardPrefab = boardPrefab;
        _gameConfig = gameConfig;
        _resPresenter = resourcePresenter;
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
                // ������� ����� ������ ������
                var cellModel = new CellModel(0, _gameConfig.maxDepth, false);
                _cellModels.Add(cellModel);

                // ������� ������������� ������
                var cellObject = Instantiate(_cellPrefab, new Vector2(x, y), Quaternion.identity);
                cellObject.name = $"Cell ({x},{y})";
                _cells.Add(cellObject);

                // �������� ������������� � ��������� ��� ���� ������
                var cellView = cellObject.GetComponent<CellView>();
                var cellPresenter = new CellPresenter(cellModel, cellView, _resPresenter, _rewardManager);

                _presenters.Add(cellPresenter);

                // �������� ��������� ��������� �� View
                cellView.UpdateDepth(cellModel.CurrentDepth, cellModel.MaxDepth);
            }
        }

        // ������������� ������ � ��������� �����
        var center = GetGridCenter();
        SetBorder(center);
    }

    public void HandleGenerateGrid(List<CellData> cellsData)
    {
        ClearGrid();

        if (cellsData != null && cellsData.Count > 0)
        {
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    int index = x * _size + y;
                    if (index >= cellsData.Count)
                    {
                        Debug.LogWarning("Insufficient data to fill the entire grid. Remaining cells will be empty.");
                        break;
                    }

                    // �������� ������ � ������
                    CellData cellData = cellsData[index];

                    // ������� ������ ������
                    var cellModel = new CellModel(cellData.depth, _gameConfig.maxDepth, cellData.hasGold);
                    _cellModels.Add(cellModel);

                    // ������� ������������� ������
                    var cellObject = Instantiate(_cellPrefab, new Vector2(x, y), Quaternion.identity);
                    cellObject.name = $"Cell ({x},{y})";
                    _cells.Add(cellObject);

                    // ����������� View � Presenter
                    var cellView = cellObject.GetComponent<CellView>();
                    var cellPresenter = new CellPresenter(cellModel, cellView, _resPresenter, _rewardManager);
                    _presenters.Add(cellPresenter);

                    // ������������� ��������� ������
                    cellView.UpdateDepth(cellModel.CurrentDepth, cellModel.MaxDepth);
                    if (cellData.hasGold)
                    {
                        // ���������� ����� ����� ��� ���������������� ������ ������
                        _rewardManager.ForceSpawnGold(cellModel, cellObject.transform);
                    }
                }
            }
        }

        // ������������� ������� � ���������� ������
        var center = GetGridCenter();
        SetBorder(center);
    }

    public void ClearGrid()
    {
        foreach (var presenter in _presenters)
        {
            presenter.Dispose();
        }

        
        // ������� ������� ������
        foreach (var cell in _cells)
        {
            Destroy(cell);
        }

        // ������� ������ �����
        if (_board != null)
        {
            Destroy(_board.gameObject);
            _board = null;
        }
        _presenters.Clear();
        _cells.Clear();
        _cellModels.Clear();
    }

    private void SetBorder(Vector2 center)
    {
        _board = Instantiate(_boardPrefab, center, Quaternion.identity);
        _board.size = new Vector2(_size, _size);
    }
    public Vector2 GetGridCenter()
    {
        return new Vector2((float)_size / 2 - 0.5f, (float)_size / 2 - 0.5f);
    }

    public List<CellData> GetCellData()
    {
        Debug.Log("Start GetCellData!");
        List<CellData> cellsData = new List<CellData>();
        foreach (var cell in _cellModels)
        {
            int curDepth = cell.CurrentDepth;
            bool hasGold = cell.HasGold;
            CellData cellData = new CellData(curDepth, hasGold);
            Debug.Log($"CurDepth: {curDepth} + hasGold: {hasGold}");
            cellsData.Add(cellData);
        }
        return cellsData;
    }
}
