using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
            if (saveData.cells == null || saveData.cells.Count == 0)
            {
                Debug.LogError("SaveGame: cells data is null or empty!");
                return;
            }

            Debug.Log("Cells data before serialization:");
            foreach (var cell in saveData.cells)
            {
                Debug.Log($"Cell: depth={cell.depth}, hasGold={cell.hasGold}");
            }

            var jsonCells = JsonConvert.SerializeObject(saveData.cells);
            Debug.Log($"Serialized CellsData: {jsonCells}");

            // Проверяем существующую запись для данного пользователя
            var existingGameState = _db.Table<GameState>().FirstOrDefault(g => g.UserID == userId);

            if (existingGameState != null)
            {
                // Если запись существует, обновляем ее
                existingGameState.Shovels = saveData.shovels;
                existingGameState.CollectedGold = saveData.collectedGold;
                existingGameState.CellsData = jsonCells;

                _db.Update(existingGameState);
                Debug.Log($"Game state for user {userId} updated successfully");
            }
            else
            {
                // Если записи нет, создаем новую
                var newGameState = new GameState
                {
                    UserID = userId,
                    Shovels = saveData.shovels,
                    CollectedGold = saveData.collectedGold,
                    CellsData = jsonCells
                };

                _db.Insert(newGameState);
                Debug.Log($"Game state for user {userId} saved successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game state for user {userId}: {e.Message}");
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
                cells = JsonConvert.DeserializeObject<List<CellData>>(gameState.CellsData)
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

    [NotNull, Unique]
    public int UserID { get; set; }

    public int Shovels { get; set; }
    public int CollectedGold { get; set; }
    
    [NotNull]
    public string CellsData { get; set; } // JSON-строка
}