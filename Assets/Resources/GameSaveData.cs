using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int shovels;
    public int collectedGold;
    public List<CellData> cells;
    
    // ����� ���� ��� ���������� ����������
    public int fieldSize;
    public int maxDepth;
    public int maxShovels;
    public int requiredGoldBars;
    public float goldSpawnChance;
    public float goldSpawnChanceIncrement;
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


