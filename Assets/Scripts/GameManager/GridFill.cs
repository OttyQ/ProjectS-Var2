using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Класс для управления инициализацией и очисткой сетки клеток.
/// Включает методы для создания клеток, загрузки данных сетки и установки золота в клетки.
/// </summary>
public class GridFill : MonoBehaviour
{
    private Transform gridParent;
    private GameObject cellPrefab;
    private int _maxDepth;
    private CountHandler _countHandler;

    List<Cell> cells = new List<Cell>();

    public void Initialize(Transform gridParent, GameObject cellPrefab, int maxDepth, CountHandler countHandler)
    {
        this.gridParent = gridParent;
        this.cellPrefab = cellPrefab;
        _maxDepth = maxDepth;
        _countHandler = countHandler;
    }

    /// <summary>
    /// Заполнение сетки клетками с заданным размером поля.
    /// </summary>
    /// <param name="fieldSize">Размер поля </param>
    public void InitializeGrid(int fieldSize)
    {
        ClearGrid();
        for (int i = 0; i < fieldSize * fieldSize; i++)
        {
            GameObject cellObject = Instantiate(cellPrefab, gridParent);
            Cell cell = cellObject.GetComponent<Cell>();
            cell.OnCellDigged += _countHandler.UseShovel;
            cell.Initialize(_maxDepth,_maxDepth, _countHandler, false);
            cells.Add(cell);   
        }
    }

    public void InitializeGridFromData(List<CellData> cellDataList, RewardManager rewardManager)
    {
        ClearGrid();

        if (cellDataList != null && cellDataList.Count > 0)
        {
            for (int i = 0; i < cellDataList.Count; i++)
            {
                CellData cellData = cellDataList[i];
                GameObject cellObject = Instantiate(cellPrefab, gridParent);
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Initialize(_maxDepth, cellData.depth, _countHandler, cellData.hasGold);
                cell.OnCellDigged += _countHandler.UseShovel;

                cells.Add(cell);
            }
        }
        else
        {
            Debug.LogWarning("No cell data to initialize");
        }
    }

    public void DataGridSpawnGold(RewardManager rewardManager)
    {
        foreach (Cell cell in cells)
        {
            if (cell.HasGold())
            {               
                rewardManager.HandleGoldSpawn(cell.transform);

                cell.AssignGold();
            }
        }
    }

    public void ClearGrid()
    {
        foreach (var cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells.Clear();
    }

    /// <summary>
    /// Получение данных о клетках для сохранения игры.
    /// </summary>
    /// <returns>Список данных о клетках, включая глубину и наличие золота.</returns>
    public List<CellData> GetCellsData()
    {
        List<CellData> cellDataList = new List<CellData>();

        foreach (var cell in cells)
        {
            bool hasGold = cell.HasGold();  
            int depth = cell.GetDepth();    

            cellDataList.Add(new CellData(depth, hasGold)); 
        }

        return cellDataList;
    }

    public Cell[] GetCells()
    {
        return cells.ToArray();
    }
}

