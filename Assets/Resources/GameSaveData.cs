using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int shovels;
    public int collectedGold;
    public List<CellData> cells;
}

[System.Serializable]
public class CellData
{
    public int depth;        // ������� ������
    public bool hasGold;     // ������� ������ � ������

    public CellData(int depth, bool hasGold)
    {
        this.depth = depth;
        this.hasGold = hasGold;
    }
}
