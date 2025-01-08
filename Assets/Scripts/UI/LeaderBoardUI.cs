using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    [SerializeField] private Transform _leaderBoardContainer; // Контейнер для элементов таблицы
    [SerializeField] private GameObject _leaderBoardEntryPrefab; // Префаб элемента таблицы
    [SerializeField] private TextMeshProUGUI _noLeaderText; // Текст о том, что список пуст

    private DataBaseManager _dbManager;

    private void Start()
    {
        // Инициализация менеджера базы данных
        _dbManager = new DataBaseManager(Application.persistentDataPath + "/game.db");

        // Отображение таблицы лидеров
        DisplayLeaderBoard();
    }

    /// <summary>
    /// Отображает таблицу лидеров.
    /// </summary>
    private void DisplayLeaderBoard()
    {
        var leaderBoardData = _dbManager.GetLeaderBoard();

        // Очистка контейнера перед заполнением
        foreach (Transform child in _leaderBoardContainer)
        {
            Destroy(child.gameObject);
        }

        if (leaderBoardData == null || leaderBoardData.Count == 0)
        {
            _noLeaderText.gameObject.SetActive(true);
            return;
        }
        
        _noLeaderText.gameObject.SetActive(false);
        
        // Создание новых элементов
        foreach (var entry in leaderBoardData)
        {
            CreateLeaderBoardEntry(entry);
        }
    }

    /// <summary>
    /// Создает и заполняет элемент таблицы лидеров.
    /// </summary>
    /// <param name="entry">Данные одной записи таблицы лидеров.</param>
    private void CreateLeaderBoardEntry(LeaderBoardEntry entry)
    {
        var instance = Instantiate(_leaderBoardEntryPrefab, _leaderBoardContainer);

        // Получаем ссылки на текстовые поля
        var usernameText = instance.transform.Find("UsernameText").GetComponent<TextMeshProUGUI>();
        var scoreText = instance.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        var timestampText = instance.transform.Find("TimestampText").GetComponent<TextMeshProUGUI>();

        // Устанавливаем данные
        usernameText.text = entry.Username;
        scoreText.text = entry.MaxScore.ToString();
        timestampText.text = entry.Timestamp.ToString("dd-MM-yyyy HH:mm"); //"yyyy-MM-dd HH:mm" 
    }
}