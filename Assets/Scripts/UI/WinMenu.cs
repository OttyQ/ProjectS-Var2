using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinMenu : MonoBehaviour, IGameMenu
{
    [SerializeField] TextMeshProUGUI scoreText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = $"{score} gold was collected";
    }
}
