using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour, IComparable<Task> {
    private const string SUCCESS_MESSAGE = "Complete! :)";

    private const string FAILURE_MESSAGE = "Out of time! :(";

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

    [SerializeField]
    private Color successColor;

    [SerializeField]
    private Color failureColor;

    /// <summary>
    /// How long to wait until the task is removed
    /// </summary>
    [SerializeField]
    private float secondsBeforeRemoval;

    /// <summary>
    /// How long to do the growth and shrink effect on add/removal.
    /// </summary>
    [SerializeField]
    private float animationTime;

    private string objectiveName;

    private float currentSeconds;

    private int totalSeconds;

    private bool isBeginRemoval;

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

    public IEnumerator PlayDestroyEffect(bool isSuccessful, Action callBack) {
        isBeginRemoval = true;

        string result = string.Empty;
        Color color = Color.red;

        if (isSuccessful) {
            result = SUCCESS_MESSAGE;
            color = successColor;
        } else {
            result = FAILURE_MESSAGE;
            color = failureColor;
        }

        ObjectiveName = string.Format("{0} - {1}", objectiveName, result);
        Color = color;

        yield return new WaitForSeconds(secondsBeforeRemoval);
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
        if (!isBeginRemoval) {
            SetTime(currentSeconds - Time.deltaTime);
        }
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