using UnityEngine;
using System.Collections;

public class timescripttest : MonoBehaviour {


	float holdTime;
	float startingHoldTime = 0.25f;
	float triggerHoldTime = 0;

	// Use this for initialization
	void Start () {
		holdTime = startingHoldTime;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (0) && holdTime >= triggerHoldTime) {
			print ("a push");
			holdTime = startingHoldTime;
		} else if (Input.GetMouseButtonUp (0) && holdTime < triggerHoldTime) {
			print ("renewed");
			holdTime = startingHoldTime;
		}

		if (Input.GetMouseButton (0)) {

			holdTime -= Time.deltaTime;

			if(holdTime<triggerHoldTime)
			print ("is holding");
		}

	}
}
