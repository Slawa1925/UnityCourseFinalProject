using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public EnemyManager EnemyManager;
    [SerializeField] private Text _scoreText;
    private int _score;
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _gameVictoryScreen;
    [SerializeField] private GameObject _gameMenu;


    private void Start()
    {
        // subscribe all actions here
        EnemyManager.PlayerManager = PlayerManager;
        EnemyManager.AddScore += AddScore;
        EnemyManager.Victory += OnVictory;

        PlayerManager.GameOver += OnGameOver;
        //OnMenu();
        OnRestart();
    }

    public void OnExit()
    {
        ExitHelper.Exit();
    }

    public void OnRestart()
    {
        Time.timeScale = 1;
        _gameScreen.SetActive(true);
        _gameOverScreen.SetActive(false);
        _gameVictoryScreen.SetActive(false);
        _gameMenu.SetActive(false);
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        _gameScreen.SetActive(false);
        _gameOverScreen.SetActive(true);
        _gameVictoryScreen.SetActive(false);
        _gameMenu.SetActive(false);
    }

    public void OnVictory()
    {
        Time.timeScale = 0;
        _gameScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _gameVictoryScreen.SetActive(true);
        _gameMenu.SetActive(false);
    }

    public void OnMenu()
    {
        Time.timeScale = 0;
        _gameScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _gameVictoryScreen.SetActive(false);
        _gameMenu.SetActive(true);
    }

    public void AddScore(int score)
    {
        _score += score;
        _scoreText.text = "" + _score;
    }
}
