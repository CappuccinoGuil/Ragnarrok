using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    [SerializeField] float boostForce = 100000f;
    [SerializeField] bool vertical = false;

    private Vector2 direction = Vector2.right;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	if (vertical)
    {
            direction = Vector2.up;

    }
    else { direction = Vector2.right; }


	}
    void OnTriggerStay2D(Collider2D thingToBoost)
    {
        if (thingToBoost.CompareTag("Ragnar") || thingToBoost.CompareTag("Interactive") || thingToBoost.CompareTag("Dwane") || thingToBoost.CompareTag("WoodenObject"))
        {
            thingToBoost.GetComponent<Rigidbody2D>().AddForce(direction * boostForce, ForceMode2D.Force);
        }
    }
}
