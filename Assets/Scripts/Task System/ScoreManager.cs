using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    private const char MISSED_MARK = '|';

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text missedText;

    private int score;
    private int missed;

    public int Score {
        get {
            return score;
        }
    }

    // TODO some fancy adding animation
    public void AddToScore(int amount) {
        score += amount;
    }

    public void AddMissedTask(int scoreToDeduct) {
        this.missed++;
    }

    private void Update() {
        this.missedText.text = missed.ToString();
        this.scoreText.text = score.ToString();
        if (score < 0) {
            scoreText.color = Color.red;
        } else {
            scoreText.color = Color.green;
        }
    }
}