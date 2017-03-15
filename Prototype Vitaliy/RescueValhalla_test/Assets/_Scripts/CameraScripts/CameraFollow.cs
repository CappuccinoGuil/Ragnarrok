using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject rock;
    float deltaPos;
    float lastRockPos=-3;
    float newRockPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        newRockPos = rock.transform.position.y;
        deltaPos = newRockPos - lastRockPos;
        transform.Translate(Vector2.up * deltaPos);
        //transform.position.y = transform.position.y + deltaPos;
        lastRockPos = lastRockPos + deltaPos;
	}
}
