using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        GamePlay,
        Paused,
        GameOver,
        LevelUP
    }
    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject LevelUPScreen;


    // Current Stats UI
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentMightDisplay;
    public TextMeshProUGUI currentProjectileSpeedDisplay;
    public TextMeshProUGUI currentMagnetDisplay;

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TextMeshProUGUI stopwatchDisplay;

    public bool isGameOver = false;
    public bool canUpgrade = false;

    public GameObject playerObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra " + this + "Deleted");
            Destroy(gameObject);

        }
        DiscableScreens();
    }


    void Update()
    {
        switch (currentState)
        {
            case GameState.GamePlay:
                CheckForPausedAndResume();
                UpdateStopwatch();
                //Code
                break;
            case GameState.Paused:
                CheckForPausedAndResume();
                //Code
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game Over!");
                    DisplayResults();

                }
                //Code
                break;
            case GameState.LevelUP:
                if (!canUpgrade)
                {
                    canUpgrade = true;
                    Time.timeScale = 0f;
                    LevelUPScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("Invalid State");
                //Code
                break;
        }
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game Paused");

        }


    }
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game Resumed");
        }

    }

    void CheckForPausedAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DiscableScreens()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        LevelUPScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);

    }

    void DisplayResults()
    {
        resultScreen.SetActive(true);




    }
    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            GameOver();
        }

    }
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void StartLevelUP()
    {
        ChangeState(GameState.LevelUP);
        playerObject.SendMessage("RemoveAndApplyUpgrades");

    }
    public void EndLevelUP()
    {
        canUpgrade = false;
        Time.timeScale = 1f;
        LevelUPScreen.SetActive(false);
        ChangeState(GameState.GamePlay);
    }


}
