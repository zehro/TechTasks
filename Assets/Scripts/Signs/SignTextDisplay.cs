using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignTextDisplay : MonoBehaviour {

    [SerializeField]
    private Text signText;

    public void ShowSignDisplay(string message) {
        signText.text = message;
        gameObject.SetActive(true);
    }

    public void HideSignDisplay() {
        gameObject.SetActive(false);
    }
}