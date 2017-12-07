using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Opens new scene
	public void changeScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
	}
		
	// Leave the main menu and go to sub menu maskName
	public void leaveMenu(string maskName)
	{
		// Get main menu mask
		GameObject oldMask = GameObject.Find("Mask");

		// Get all children of the canvas
		// This is necessary because sub menus are all inactive so they cannot be found just using the Find()
		// method and the name
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);

		// Loop through children until the correct mask is found
		foreach (Transform t in trans) {
			if (t.gameObject.name == maskName) {
				// Set main menu to inactive
				oldMask.SetActive (false);

				// Set sub menu to active
				t.gameObject.SetActive (true);
			}
		}
	}


	// Leave the current sub menu maskName, which needs to be passed as a parameter
	// and go back to the main menu
	public void backToMenu(string maskName)
	{
		// Get current sub menu
		GameObject oldMask = GameObject.Find(maskName);

		// Get all children of the canvas
		// This is necessary because all masks are inactive, except the current one 
		// So they cannot be found just using the Find() method and the name
		Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);

		// Loop through children until the correct mask is found
		foreach (Transform t in trans) {
			if (t.gameObject.name == "Mask") {
				// Set sub menu to inactive
				oldMask.SetActive (false);

				// Set main menu to active
				t.gameObject.SetActive (true);
			}
		}
	}

	public void quitGame()
	{
		Application.Quit ();
	}
}