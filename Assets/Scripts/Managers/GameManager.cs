using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private State state;
    private int collectCoinCount;
    private float countdownToStartTimer = 3f;
    private bool isGamePaused;
    public event EventHandler OnStateChange;
    public event EventHandler OnPauseGame;
    public event EventHandler OnUpdateCollectedCoinCount;


    public enum State
    {
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are multiple Game Instances");
        }
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
        state = State.CountdownToStart;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    private void Update()

    {
        switch (state)
        {
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    countdownToStartTimer = 3f;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                break;
            case State.GameOver:
                break;
        }
    }


    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public void IncrementCoinCount()
    {
        collectCoinCount++;
        OnUpdateCollectedCoinCount?.Invoke(this, EventArgs.Empty);
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public int GetCollectedCoin()
    {
        return collectCoinCount;
    }

    public void SetGameOver()
    {
        state = State.GameOver;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        state = State.CountdownToStart;
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ToogleGamePause()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            state = State.CountdownToStart;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            isGamePaused = true;
            Time.timeScale = 0f;
        }
        OnPauseGame?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }
    
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
}
