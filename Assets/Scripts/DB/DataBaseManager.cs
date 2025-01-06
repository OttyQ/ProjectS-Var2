using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DataBaseManager
{
    private readonly SQLiteConnection _db;

    public DataBaseManager(string dbPath)
    {
        _db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _db.CreateTable<GameState>();
    }

    public void SaveGame(int userId, GameSaveData saveData)
    {
        try
        {
            var gameState = new GameState
            {
                UserID = userId,
                Shovels = saveData.shovels,
                CollectedGold = saveData.collectedGold,
                CellsData = JsonUtility.ToJson(saveData.cells)
            };

            _db.InsertOrReplace(gameState);
            Debug.Log("Game state saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game state: {e.Message}");
        }
    }

    public GameSaveData LoadGame(int userId)
    {
        try
        {
            var gameState = _db.Table<GameState>().FirstOrDefault(g => g.UserID == userId);
            if (gameState == null) return null;
            return new GameSaveData
            {
                shovels = gameState.Shovels,
                collectedGold = gameState.CollectedGold,
                cells = JsonUtility.FromJson<List<CellData>>(gameState.CellsData)
            };
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game state: {e.Message}");
            return null;
        }
    }
    
    
}

[Table("GameState")]
public class GameState
{
    [PrimaryKey, AutoIncrement]
    public int SaveID { get; set; }

    [NotNull]
    public int UserID { get; set; }

    public int Shovels { get; set; }
    public int CollectedGold { get; set; }
    public string CellsData { get; set; } // JSON-строка
}