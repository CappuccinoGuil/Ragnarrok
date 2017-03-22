using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransition : MonoBehaviour {




	//Attach this script to any button and just type in the scene name as a string.
	public void NextLevelButton(string LevelName)
	{
		
		Application.LoadLevel (LevelName);
	}


}
