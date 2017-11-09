using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedText : MonoBehaviour {

	public float letterPaused = 0.01f;
	private string message;
	public Text textComp;

	// Use this for initialization
	void Start ()
	{
		textComp = GetComponent<Text> ();
		message = textComp.text;
		textComp.text = "";
		StartCoroutine (TypeText ());
	}

	IEnumerator TypeText()
	{
		foreach (char letter in message.ToCharArray()) 
		{
			textComp.text += letter;
			yield return 0;
			yield return new WaitForSeconds(letterPaused);
		}
	}

}
