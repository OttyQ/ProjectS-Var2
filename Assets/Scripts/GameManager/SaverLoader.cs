using System.IO;
using UnityEngine;

public class SaverLoader : MonoBehaviour
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    /// <summary>
    /// Сохраняет данные игры в файл.
    /// </summary>
    /// <param name="saveData">Объект, содержащий данные для сохранения.</param>
    public void SaveGame(GameSaveData saveData)
    {
        Debug.Log("Write to save");
        Debug.Log($"Save path:{SavePath}");
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SavePath, json);
    }

    /// <summary>
    /// Загружает данные игры из файла.
    /// </summary>
    /// <returns>Объект с загруженными данными игры или null, если файл не найден.</returns>
    public GameSaveData LoadGame()
    {
        Debug.Log("Loading a save...");
        if (!File.Exists(SavePath))
        {
            Debug.Log("File not found!");
            return null;
        }
        Debug.Log("File found!");
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    /// <summary>
    /// Удаляет файл сохранения, если он существует.
    /// </summary>
    public void DeleteSaveFile()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("The save file was deleted.");
        }
        else
        {
            Debug.Log("File not found!");
        }
    }

}
