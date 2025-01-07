using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;
using System.Linq;
using SQLite;


public class DataBaseManager
{
    // Соединение с базой данных
    private readonly SQLiteConnection _db;

    /// <summary>
    /// Конструктор для инициализации соединения с базой данных.
    /// </summary>
    /// <param name="dbPath">Путь к базе данных</param>
    public DataBaseManager(string dbPath)
    {
        // Открытие или создание базы данных
        _db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        
        _db.Execute("PRAGMA foreign_keys = ON;"); // Включение поддержки внешних ключей
        CreateGsTableWithForeignKey();
        CreateLbTableWithForeignKey();
    }

    private void CreateGsTableWithForeignKey()
    {
        _db.Execute($@"
        CREATE TABLE IF NOT EXISTS GameState (
            SaveID INTEGER PRIMARY KEY AUTOINCREMENT,
            UserID INTEGER NOT NULL,
            Shovels INTEGER,
            MaxShovels INTEGER,
            CollectedGold INTEGER,
            CellsData TEXT NOT NULL,
            FieldSize INTEGER,
            MaxDepth INTEGER,
            RequiredGoldBars INTEGER,
            GoldSpawnChance REAL,
            GoldSpawnChanceIncrement REAL,
            FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
        );
    ");
    }

    private void CreateLbTableWithForeignKey()
    {
        _db.Execute($@"
        CREATE TABLE IF NOT EXISTS LeaderBoard (
            LeaderBoardID INTEGER PRIMARY KEY AUTOINCREMENT,
            UserID INTEGER NOT NULL,
            MaxScore INTEGER NOT NULL,
            Timestamp TEXT NOT NULL,
            FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
        );
        ");
    }

    /// <summary>
    /// Сохраняет текущее состояние игры в базу данных для заданного пользователя.
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <param name="saveData">Данные игры для сохранения</param>
    public void SaveGame(int userId, GameSaveData saveData)
    {
        try
        {
            // Проверяем, что данные клеток не пусты
            if (saveData.cells == null || saveData.cells.Count == 0)
            {
                Debug.LogError("SaveGame: cells data is null or empty!");
                return;
            }

            // Сериализация данных клеток в JSON
            var jsonCells = JsonConvert.SerializeObject(saveData.cells);

            // Проверяем, существует ли запись для данного пользователя
            var existingGameState = _db.Table<GameState>().FirstOrDefault(g => g.UserID == userId);

            if (existingGameState != null)
            {
                // Обновляем существующую запись
                existingGameState.Shovels = saveData.shovels;
                existingGameState.MaxShovels = saveData.maxShovels;
                existingGameState.CollectedGold = saveData.collectedGold;
                existingGameState.CellsData = jsonCells;
                existingGameState.FieldSize = saveData.fieldSize;
                existingGameState.MaxDepth = saveData.maxDepth;
                existingGameState.RequiredGoldBars = saveData.requiredGoldBars;
                existingGameState.GoldSpawnChance = saveData.goldSpawnChance;
                existingGameState.GoldSpawnChanceIncrement = saveData.goldSpawnChanceIncrement;

                _db.Update(existingGameState); // Обновляем запись в базе
                Debug.Log($"Game state for user {userId} updated successfully");
            }
            else
            {
                // Создаем новую запись
                var newGameState = new GameState
                {
                    UserID = userId,
                    Shovels = saveData.shovels,
                    MaxShovels = saveData.maxShovels,
                    CollectedGold = saveData.collectedGold,
                    CellsData = jsonCells,
                    FieldSize = saveData.fieldSize,
                    MaxDepth = saveData.maxDepth,
                    RequiredGoldBars = saveData.requiredGoldBars,
                    GoldSpawnChance = saveData.goldSpawnChance,
                    GoldSpawnChanceIncrement = saveData.goldSpawnChanceIncrement
                };

                _db.Insert(newGameState); // Вставляем новую запись в базу
                Debug.Log($"Game state for user {userId} saved successfully");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game state for user {userId}: {e.Message}");
        }
    }

    /// <summary>
    /// Загружает состояние игры из базы данных для заданного пользователя.
    /// </summary>
    /// <param name="userId">ID пользователя</param>
    /// <returns>Объект GameSaveData с сохраненными данными игры</returns>
    public GameSaveData LoadGame(int userId)
    {
        try
        {
            // Ищем запись в таблице GameState по UserID
            var gameState = _db.Table<GameState>().FirstOrDefault(g => g.UserID == userId);
            if (gameState == null) return null; // Если записи нет, возвращаем null

            // Возвращаем десериализованные данные игры
            return new GameSaveData
            {
                shovels = gameState.Shovels,
                maxShovels = gameState.MaxShovels,
                collectedGold = gameState.CollectedGold,
                cells = JsonConvert.DeserializeObject<List<CellData>>(gameState.CellsData),
                fieldSize = gameState.FieldSize,
                maxDepth = gameState.MaxDepth,
                requiredGoldBars = gameState.RequiredGoldBars,
                goldSpawnChance = gameState.GoldSpawnChance,
                goldSpawnChanceIncrement = gameState.GoldSpawnChanceIncrement
            };
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game state: {e.Message}");
            return null;
        }
    }

    public void UpdateLeaderBoard(int userId, int score)
    {
        try
        {
            // Проверяем текущий рекорд пользователя
            var existingRecord = _db.Table<LeaderBoard>().FirstOrDefault(lb => lb.UserID == userId);

            if (existingRecord != null)
            {
                if (score > existingRecord.MaxScore)
                {
                    // Обновляем рекорд
                    existingRecord.MaxScore = score;
                    existingRecord.Timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601 формат
                    _db.Update(existingRecord);
                    Debug.Log($"LeaderBoard updated for user {userId} with new score: {score}");
                }
            }
            else
            {
                // Создаем новую запись
                var newRecord = new LeaderBoard
                {
                    UserID = userId,
                    MaxScore = score,
                    Timestamp = DateTime.UtcNow.ToString("o")
                };
                _db.Insert(newRecord);
                Debug.Log($"New LeaderBoard record created for user {userId} with score: {score}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to update LeaderBoard for user {userId}: {e.Message}");
        }
    }

    public List<LeaderBoardEntry> GetLeaderBoard()
    {
        var query = @"
        SELECT u.Username, lb.MaxScore, lb.Timestamp 
        FROM LeaderBoard lb
        INNER JOIN Users u ON lb.UserID = u.UserID
        ORDER BY lb.MaxScore DESC
        LIMIT 10;";

        var entries = _db.Query<LeaderBoardEntryQueryResult>(query);

        // Конвертируем результат запроса в список LeaderBoardEntry
        var leaderBoardEntries = new List<LeaderBoardEntry>();
        foreach (var entry in entries)
        {
            leaderBoardEntries.Add(new LeaderBoardEntry
            {
                Username = entry.Username,
                MaxScore = entry.MaxScore,
                Timestamp = System.DateTime.Parse(entry.Timestamp)
            });
        }

        return leaderBoardEntries;
    }
}


/// <summary>
/// Модель таблицы для хранения состояния игры.
/// </summary>
[Table("GameState")]
public class GameState
{
    [PrimaryKey, AutoIncrement]
    public int SaveID { get; set; } // Уникальный идентификатор сохранения

    [NotNull, Unique]
    public int UserID { get; set; } // Идентификатор пользователя

    public int Shovels { get; set; } // Количество лопат
    public int MaxShovels { get; set; } // Максимальное количество лопат
    public int CollectedGold { get; set; } // Собранное золото

    [NotNull]
    public string CellsData { get; set; } // JSON-строка с данными клеток

    public int FieldSize { get; set; } // Размер поля
    public int MaxDepth { get; set; } // Максимальная глубина
    public int RequiredGoldBars { get; set; } // Необходимое золото для победы
    public float GoldSpawnChance { get; set; } // Шанс появления золота
    public float GoldSpawnChanceIncrement { get; set; } // Увеличение шанса появления золота
}

[Table("LeaderBoard")]
public class LeaderBoard
{
    [PrimaryKey, AutoIncrement]
    public int LeaderBoardID { get; set; } // Уникальный идентификатор записи

    [NotNull]
    public int UserID { get; set; } // Внешний ключ для связи с таблицей Users

    [NotNull]
    public int MaxScore { get; set; } // Максимальный счет пользователя

    [NotNull]
    public string Timestamp { get; set; } // Дата и время установки рекорда (в формате ISO 8601)
}