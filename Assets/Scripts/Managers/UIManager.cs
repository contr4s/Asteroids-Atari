using System;
using TMPro;
using UnityEngine;

[Serializable]
public class UIManager
{
    [SerializeField] private GameObject[] UILives;

    [SerializeField] private TextMeshProUGUI scoreBoard;
    [SerializeField] private TextMeshProUGUI levelBoard;

    [SerializeField]
    private float _timeOfShowingPreGamePanel = 1f;
    public float TimeOfShowingPreGamePanel
    {
        get => _timeOfShowingPreGamePanel;
        private set => _timeOfShowingPreGamePanel = value;
    }

    [SerializeField]
    private GameObject _pregameUI;

    public GameObject PregameUI
    {
        get => _pregameUI;
        private set => _pregameUI = value;
    }

    [SerializeField]
    private GameObject _gameOverUI;
    public GameObject GameOverUI
    {
        get => _gameOverUI;
        private set => _gameOverUI = value;
    }

    public void RefreshUILives(int lives)
    {
        for (int i = 0; i < UILives.Length; i++)
        {
            if (i < lives)
            {
                UILives[i].SetActive(true);
            }
            else
            {
                UILives[i].SetActive(false);
            }
        }
    }

    public void UpdateScoreBoard(string text)
    {
        scoreBoard.text = text;
    }

    public void UpdateLevelBoard(string text)
    {
        scoreBoard.text = text;
    }
}

