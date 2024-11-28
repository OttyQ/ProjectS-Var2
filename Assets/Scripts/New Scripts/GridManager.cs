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
    private RewardManager _rewardManager;

    private List<GameObject> _cells; // ������ ���� ��������� ������ (�������������)
    private List<CellModel> _cellModels; // ������ ������ ������� ������
    private List<CellPresenter> _presenters;

    public void Initialize(int size, GameObject cellPrefab, SpriteRenderer boardPrefab, Config gameConfig, IResourceHandler resourcePresenter, RewardManager rewardManager)
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
        var center = new Vector2((float)_size / 2 - 0.5f, (float)_size / 2 - 0.5f);
        SetBorder(center);
        CenterCamera(center);
    }

    public void ClearGrid()
    {
        foreach (var presenter in _presenters)
        {
            presenter.Dispose();
        }

        _presenters.Clear();
        _cells.Clear();
        _cellModels.Clear();

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
    }

    private void SetBorder(Vector2 center)
    {
        _board = Instantiate(_boardPrefab, center, Quaternion.identity);
        _board.size = new Vector2(_size, _size);
    }

    private void CenterCamera(Vector2 center)
    {
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
    }
}
