using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {


	//Declared Variables
	public GameObject platform;
	public float moveSpeed;
	private Transform currentPoint;

	//
	public int pointSelection;

	//Array of points
	public Transform[] points;

	//Booleans
	bool leverOn;




	void Start()
	{

		currentPoint = points[pointSelection]; // Point selection starts at "0";
	}


	void Update()
	{

		//Platform will move towards it's new position
		platform.transform.position = Vector3.MoveTowards
			(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

		if (platform.transform.position == currentPoint.position) 
		{

			//Increment the point to go to the next element.
			pointSelection++;

			//Check to make sure that once the point selection hits the end of the array, it will start over.
			if (pointSelection == points.Length) 
			{
				pointSelection = 0; //this will cause the platform to go back to it's intial position.
			}
		
		
			currentPoint = points [pointSelection]; 
		
		}


	}

	//Going to need a function to check and to make sure that the player stays on the platform.
	// ........Code.........







	//Lever function
	void OnTriggerEnter(Collider other)
	{
		//Turn on lever
		leverOn = true;

		if (other.gameObject.tag == "Player") {
			
			//Activate platform to move here or switch to activate a door...
		
		
		}
	}


}
