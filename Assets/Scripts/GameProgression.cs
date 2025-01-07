using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression
{
    private readonly Config _baseConfig;

    public int FieldSize { get; private set; }
    public int InitialShovelCount { get; private set; }
    public int MaxShovelCount { get; private set; }
    public int MaxDepth { get; private set; }
    public int RequiredGoldBars { get; private set; }
    public float GoldSpawnChance { get; private set; }
    public float GoldSpawnChanceIncrement { get; private set; }
    
    public GameProgression(Config baseConfig)
    {
        _baseConfig = baseConfig;
        ResetProgression();
    }
    
    public void IncreaseProgression()
    {
        if (FieldSize < 5) FieldSize += 1; // Увеличиваем поле на 1
       
        if(MaxDepth < 7) MaxDepth += 1;
        
        MaxShovelCount += 3;
        InitialShovelCount = MaxShovelCount; // Увеличиваем количество лопат
        RequiredGoldBars += 1; // Увеличиваем требуемое золото
        if(GoldSpawnChance > 0.2f) GoldSpawnChance -= 0.05f; // Уменьшаем шанс спавна золота
        
    }

    public void ResetProgression()
    {
        FieldSize = _baseConfig.FieldSize;
        MaxDepth = _baseConfig.MaxDepth;
        MaxShovelCount = _baseConfig.InitialShovelCount;
        InitialShovelCount = _baseConfig.InitialShovelCount;
        RequiredGoldBars = _baseConfig.RequiredGoldBars;
        GoldSpawnChance = _baseConfig.GoldSpawnChance;
        GoldSpawnChanceIncrement = _baseConfig.GoldSpawnChanceIncrement;
    }

    public void InitializeFromSave(GameSaveData saveData)
    {
        FieldSize = saveData.fieldSize;
        InitialShovelCount = saveData.shovels;
        MaxShovelCount = saveData.maxShovels;
        
        MaxDepth = saveData.maxDepth;
        RequiredGoldBars = saveData.requiredGoldBars;
        GoldSpawnChance = saveData.goldSpawnChance;
        GoldSpawnChanceIncrement = saveData.goldSpawnChanceIncrement;
    }
}
