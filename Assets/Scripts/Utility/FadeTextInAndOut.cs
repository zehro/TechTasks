using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextInAndOut : MonoBehaviour {

    [SerializeField]
    private Text text;

    [SerializeField]
    private float secondsPerInterval;

    private Coroutine fadeRoutine;

    private float normalTime;

    private void Start() {
        this.normalTime = Time.deltaTime;
    }

    private void OnEnable() {
        fadeRoutine = StartCoroutine(fadeInAndOut());
    }

    private void OnDisable() {
        StopCoroutine(fadeRoutine);
    }

    private IEnumerator fadeInAndOut() {
        float timer = 0;
        while (true) {
            while ((timer += normalTime) < secondsPerInterval) {
                text.color = ChangeAlpha(text.color, Mathf.Lerp(0, 1, timer / secondsPerInterval));
                yield return null;
            }
            while ((timer -= normalTime) > 0) {
                text.color = ChangeAlpha(text.color, Mathf.Lerp(1, 0, (secondsPerInterval - timer) / secondsPerInterval));
                yield return null;
            }
            yield return null;
        }
    }

    private Color ChangeAlpha(Color color, float newAlpha) {
        return new Color(color.r, color.g, color.b, newAlpha);
    }
}