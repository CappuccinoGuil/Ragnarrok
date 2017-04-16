using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointlightcript : MonoBehaviour {

	int green;

	// Use this for initialization
	void Start () {
		StartCoroutine (_changeColour ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator _changeColour (){
		green = Random.Range (10, 70);
		//print (green);
		GetComponent<Light> ().color = new Color (254f, (float)green/100, 0f);
		yield return new WaitForSeconds(0.5f);
		StartCoroutine (_changeColour ());
	}
}
