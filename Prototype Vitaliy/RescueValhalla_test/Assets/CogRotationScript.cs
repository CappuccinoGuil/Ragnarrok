using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogRotationScript : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * speed);
		//print ("Hello");
	}
}
