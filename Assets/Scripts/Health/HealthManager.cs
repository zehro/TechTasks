using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    [SerializeField]
    private Text text;

    [SerializeField]
    private Image circle;

    [SerializeField]
    private int startingHealth;

    [SerializeField]
    private Color healthy;

    [SerializeField]
    private Color dead;

    [SerializeField]
    private PauseMenuManager pause;

    private int currentHealth;

    private int Health {
        set {
            this.circle.fillAmount = ((float)value / startingHealth);
            this.circle.color = Color.Lerp(dead, healthy, circle.fillAmount);
            this.currentHealth = value;
            this.text.text = value.ToString();
        }
        get {
            return currentHealth;
        }
    }

    private void Start() {
        this.Health = startingHealth;
    }

    public void LoseHealth() {
        Health--;
        if (Health <= 0) {
            pause.DoGameOver("You died!\n(Hint: Grab stars to restore health!)");
        }
    }

    public void GainHealth() {
        if (Health < startingHealth) {
            Health++;
        }
    }
}