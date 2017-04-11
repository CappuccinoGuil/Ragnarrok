using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour {

	//Attach this script to any button and just type in the scene name as a string.
	public void NextLevelButton(string LevelName)
	{		
		SceneManager.LoadScene(LevelName);
	}
}
