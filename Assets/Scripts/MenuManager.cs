using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void changeScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}

	public void goToMask(string newMaskName)
	{
		GameObject mainMenuMask = GameObject.Find("Mask");
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);

		// Find and set currentMask to Active,
		// Set mainMenuMask to inActive
		foreach (Transform t in trans) {
			if (t.gameObject.name == newMaskName) {
				mainMenuMask.SetActive (false);
				t.gameObject.SetActive (true);
			}
		}
	}

	public void backToMainMenu(string currentMaskName)
	{
		GameObject currentMenuMask = GameObject.Find(currentMaskName);
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);

		// Set currentMask to inActive,
		// Find and set mainMenuMask to Active
		foreach (Transform t in trans) {
			if (t.gameObject.name == "Mask") {
				currentMenuMask.SetActive (false);
				t.gameObject.SetActive (true);
			}
		}
	}

	public void quitGame()
	{
		Application.Quit ();
	}
}