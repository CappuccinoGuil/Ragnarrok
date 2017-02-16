using UnityEngine;
using System.Collections;

public class playerControllerScript_p2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			transform.position = new Vector3(transform.position.x - 0.01f,transform.position.y,0f);
		}else if(Input.GetKey(KeyCode.D)){
			transform.position = new Vector3(transform.position.x + 0.01f,transform.position.y,0f);
		}
	}
}
