using UnityEngine;
using System.Collections;

public class CameraFollowingScript : MonoBehaviour {

	public GameObject target;
	float targetPosition_X;
	float targetPosition_Y;
	Vector3 currentPosition;
	Vector3 newPosition;
	float cameraOffset = 1f;

	// Use this for initialization
	void Start () {
		

	
	}
	
	// Update is called once per frame
	void Update () {
		currentPosition = transform.position;
		targetPosition_X = target.transform.position.x + cameraOffset;
		targetPosition_Y = target.transform.position.y + cameraOffset;
		newPosition = new Vector3(targetPosition_X, targetPosition_Y, transform.position.z);

		transform.position = Vector3.Lerp(currentPosition, newPosition, 1);
	}
}
