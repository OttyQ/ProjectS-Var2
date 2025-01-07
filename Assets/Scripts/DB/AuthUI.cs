using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private TMP_Text _errorText;
    
    private UserManager _userManager;

    private void Start()
    {
        _userManager = new UserManager(Application.persistentDataPath + "/game.db");
        _loginButton.onClick.AddListener(Login);
        _registerButton.onClick.AddListener(Register);
    }

    private void Register()
    {
        // Проверка на пустые поля
        if (string.IsNullOrWhiteSpace(_usernameInput.text) || string.IsNullOrWhiteSpace(_passwordInput.text))
        {
            Debug.LogError("Registration failed. Username or password is empty.");
            ShowMessage("Please fill in both username and password.");
            return; // Прерываем выполнение, если поля пустые
        }

        // Попытка регистрации
        bool success = _userManager.Register(_usernameInput.text, _passwordInput.text);
        if (success)
        {
            int userId = _userManager.Login(_usernameInput.text, _passwordInput.text); // Получаем userId после регистрации
            if (userId != 0)
            {
                Debug.Log("Registration successful");
                OnLoginSuccess(userId);
            }
        }
        else
        {
            Debug.LogError("Registration failed. Username might already exist!");
            ShowMessage("Registration failed. Username might already exist!");
        }
    }


    private void Login()
    {
        int userId = _userManager.Login(_usernameInput.text, _passwordInput.text);
        if (userId != -1)
        {
            Debug.Log($"Login successful! UserID: {userId}");
            OnLoginSuccess(userId);
        }
        else
        {
            Debug.LogError("Invalid username or password.");
            ShowMessage("Invalid username or password.");
        }
    }

    private void OnLoginSuccess(int userId)
    {
       //сохраняем userId в PlayerPrefs
       PlayerPrefs.SetInt("UserId", userId);
       PlayerPrefs.Save();
       
       //переходим на сцену с игрой
       Debug.Log($"User {userId} logged in. Loading game scene...");
       SceneManager.LoadScene("Game");
    }

    private void ShowMessage(string message)
    {
        
        _errorText.text = message;
        _errorText.color = Color.red;
        _errorText.gameObject.SetActive(true); // Убедимся, что текст отображается
        StartCoroutine(HideMessageAfterDelay(_errorText, 20f)); // Запускаем сопрограмму для скрытия
    }

    private IEnumerator HideMessageAfterDelay(TMP_Text messageText, float delay)
    {
        yield return new WaitForSeconds(delay); // Ждем указанное время
        messageText.text = ""; // Очищаем текст
    }
}
