using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour {
    private const int DEFAULT_TIME = 1;
    private const int PAUSE_TIME = 0;

    [SerializeField]
    private GameObject pauseMenu;

    private bool isGameRunning;

    private void Start() {
        isGameRunning = true;
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameRunning) {
                Time.timeScale = PAUSE_TIME;
            } else {
                Time.timeScale = DEFAULT_TIME;
            }
            pauseMenu.SetActive(isGameRunning);
            isGameRunning = !isGameRunning;
        }
    }
}