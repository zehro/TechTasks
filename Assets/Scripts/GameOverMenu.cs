using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {

    [SerializeField]
    private Text text;

    public string Header {
        set {
            this.text.text = value;
        }
    }

    public void Retry() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Test_Controller Andy");
    }
}