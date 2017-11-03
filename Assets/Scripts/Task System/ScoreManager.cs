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

    // TODO some fancy adding animation
    public void AddToScore(int amount) {
        score += amount;
    }

    public void AddMissedTask() {
        this.missed++;
        missedText.text += MISSED_MARK;
    }

    private void Update() {
        this.scoreText.text = score.ToString();
    }
}