using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    [SerializeField] float boostForce = 100000f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay2D(Collider2D thingToBoost)
    {
        thingToBoost.GetComponent<Rigidbody2D>().AddForce(Vector2.left * boostForce, ForceMode2D.Force);
    }
}
