using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [SerializeField]
    private Text text;

    private float currentTime;

    // Update is called once per frame
    private void Update() {
        currentTime += Time.deltaTime;
        text.text = GetTimeFormatted();
    }

    public string GetTimeFormatted() {
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}