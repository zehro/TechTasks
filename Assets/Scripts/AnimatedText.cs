using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedText : MonoBehaviour {
    public float letterPaused = 0.01f;
    private string message;
    public Text textComp;

    // Use this for Running Credits Scroller
    private void OnEnable() {

    // Use this for initialization
    private void Start() {
		// Gets the text to write out
        textComp = GetComponent<Text>();

		// Get the text as a string
        message = textComp.text;

		// Set text to nothing
        textComp.text = "";

		// Start automatically typing out the text on screen
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText() {
		// Write each character one at a time to the text on screen
        foreach (char letter in message.ToCharArray()) {
            textComp.text += letter;
            yield return new WaitForSeconds(letterPaused);
        }
    }
}