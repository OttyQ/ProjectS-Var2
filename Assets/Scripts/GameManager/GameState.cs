using System;
using UnityEngine;


/// <summary>
/// Управляет состоянием игры, включая победу и перезапуск.
/// </summary>
public class GameState: MonoBehaviour
{

    public event Action OnGameRestart;

    private WinMenu _winMenu;
    private CountHandler _countHandler;

    public void Initialize(WinMenu winMenu, CountHandler countHandler)
    {
        if (winMenu == null || countHandler == null)
        {
            Debug.LogError("GameState initialization failed: winMenu or countHandler is null.");
            return;
        }

        _winMenu = winMenu;
        _countHandler = countHandler;
        Debug.Log("GameState successfully initialized.");
    }

    public void Win()
    {
        Debug.Log("Player has won the game.");
        _winMenu.Setup(_countHandler.GetCollectedRewards());
        _winMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Скрывает меню победы и уведомляет подписчиков о перезапуске.
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restarting the game.");
        _winMenu.gameObject.SetActive(false);
        OnGameRestart?.Invoke();
    }
}
