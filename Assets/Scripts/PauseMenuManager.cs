using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    public enum PauseType {
        NOT_PAUSED,
        SIGN,
        MENU
    }

    private const int DEFAULT_TIME = 1;

    private const int PAUSE_TIME = 0;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private string pauseButton;

    [SerializeField]
    private SignUIManager sign;

    [SerializeField]
    private GameOverMenu gameOverMenu;

    [SerializeField]
    private ScoreManager score;

    private bool isGameRunning;

    private PauseType pauseType;

    public PauseType Pause {
        get {
            return pauseType;
        }
        set {
            this.pauseType = value;
        }
    }

    public void GoToMainMenu() {
        Time.timeScale = DEFAULT_TIME;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings() {
        Debug.Log("No settings yet.");
    }

    public void DoGameOver(string text) {
        if (isGameRunning) {
            TogglePause(() => {
                this.pauseType = PauseType.MENU;
                gameOverMenu.Header = text;
                gameOverMenu.gameObject.SetActive(true);
            });
        }
    }

    private void TogglePause() {
        if (this.pauseType == PauseType.NOT_PAUSED) {
            this.pauseType = PauseType.MENU;
        } else {
            this.pauseType = PauseType.NOT_PAUSED;
        }
        TogglePause(() => pauseMenu.SetActive(!isGameRunning));
    }

    public void TogglePause(Action postCall) {
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
        if (Input.GetButtonDown(pauseButton) && pauseType != PauseType.SIGN) {
            TogglePause();
        }
    }
}