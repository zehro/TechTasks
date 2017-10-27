using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour, IComparable<Task> {

    [SerializeField]
    private Image timeCircle;

    [SerializeField]
    private Text objective;

    [SerializeField]
    private Text time;

    [SerializeField]
    private Color initialColor;

    [SerializeField]
    private Color endColor;

    /// <summary>
    /// How long to do the growth and shrink effect on add/removal.
    /// </summary>
    [SerializeField]
    private float animationTime;

    private string objectiveName;

    private float currentSeconds;

    private int totalSeconds;

    public bool IsExpired {
        get {
            return currentSeconds <= 0;
        }
    }

    public float SecondsRemaining {
        get {
            return currentSeconds;
        }
    }

    private string ObjectiveName {
        set {
            this.objective.text = value;
            this.objectiveName = value;
        }
        get {
            return objectiveName;
        }
    }

    private float Seconds {
        set {
            string secondsText = string.Empty;
            if (value > 0) {
                secondsText = ((int)value).ToString();
            } else {
                secondsText = "0";
            }

            this.time.text = secondsText;
            this.currentSeconds = value;
            this.CircleFill = (value / totalSeconds);
        }
        get {
            return currentSeconds;
        }
    }

    private float CircleFill {
        set {
            this.timeCircle.fillAmount = value;
            Color = Color.Lerp(endColor, initialColor, value);
        }
    }

    private Color Color {
        set {
            timeCircle.color = value;
            objective.color = value;
        }
    }

    public void InitializeValues(string objectiveName, int totalSecondsToComplete) {
        this.ObjectiveName = objectiveName;
        this.totalSeconds = totalSecondsToComplete;
        this.Seconds = totalSecondsToComplete;
        this.CircleFill = 1;
    }

    public IEnumerator PlayAddEffect() {
        yield return PlayScalingEffect(Vector3.zero, Vector3.one);
    }

    public IEnumerator PlayDestroyEffect(Action callBack) {
        yield return PlayScalingEffect(Vector3.one, Vector3.zero);
        callBack();
    }

    public int CompareTo(Task other) {
        // TODO sort first by essential, then by remaining?
        return this.SecondsRemaining.CompareTo(other.SecondsRemaining);
    }

    private void SetTime(float currentSeconds) {
        this.Seconds = currentSeconds;
    }

    private void Update() {
        SetTime(currentSeconds - Time.deltaTime);
    }

    private IEnumerator PlayScalingEffect(Vector3 initialSize, Vector3 endSize) {
        transform.localScale = initialSize;
        float timer = 0;
        while ((timer += Time.deltaTime) < animationTime) {
            transform.localScale = Vector3.Lerp(initialSize, endSize, timer / animationTime);
            yield return null;
        }
        transform.localScale = endSize;
    }
}