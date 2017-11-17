using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
    private const string GAME_OVER_TEXT = "You obtained all the stars in {0}! Wow!";
    private const int DEFAULT_TIME = 1;

    private const int PAUSE_TIME = 0;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameOverMenu gameOverMenu;

    [SerializeField]
    private ScoreManager score;

    private bool isGameRunning;

    public void GoToMainMenu() {
        Time.timeScale = DEFAULT_TIME;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings() {
        Debug.Log("No settings yet.");
    }

    public void DoGameOver(string gameEndTime) {
        if (isGameRunning) {
            TogglePause(() => {
                gameOverMenu.Header = string.Format(GAME_OVER_TEXT, gameEndTime);
                gameOverMenu.gameObject.SetActive(true);
            });
        }
    }

    public void TogglePause() {
        TogglePause(() => pauseMenu.SetActive(!isGameRunning));
    }

    private void TogglePause(Action postCall) {
        if (isGameRunning) {
            Time.timeScale = PAUSE_TIME;
        } else {
            Time.timeScale = DEFAULT_TIME;
        }
        Cursor.visible = isGameRunning;
        Cursor.lockState = isGameRunning ? CursorLockMode.None : CursorLockMode.Locked;
        isGameRunning = !isGameRunning;
        postCall();
    }

    private void Start() {
        isGameRunning = true;
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }
}