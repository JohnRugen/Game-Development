using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Navigation manager, allows the user to traverse the game scenes
public class NavigationManager : MonoBehaviour {
	// Make an instance of this
	public static NavigationManager Instance;

	// Load a scene depending on the string passed in.
	public void LoadScene(string level)
	{
		SceneManager.LoadScene(level, LoadSceneMode.Single);
	}

}
