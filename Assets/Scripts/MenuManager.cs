using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void changeScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}

	public void leaveMenu(string maskName)
	{
		GameObject oldMask = GameObject.Find("Mask");
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
		print (oldMask.name);
		print (maskName);
		foreach (Transform t in trans) {
			if (t.gameObject.name == maskName) {
				print (t.gameObject.name);
				oldMask.SetActive (false);
				t.gameObject.SetActive (true);
			}
		}
	}

	public void backToMenu(string maskName)
	{
		GameObject oldMask = GameObject.Find(maskName);
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
		print (oldMask.name);
		print (maskName);
		foreach (Transform t in trans) {
			if (t.gameObject.name == "Mask") {
				print (t.gameObject.name);
				oldMask.SetActive (false);
				t.gameObject.SetActive (true);
			}
		}
	}

}