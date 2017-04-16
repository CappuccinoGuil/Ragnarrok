using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightUpInRangeScript : MonoBehaviour {

	[SerializeField] float m_effectRadius = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		LightUpPlatformsInRange ();
	}

	//Hallo effect
	void LightUpPlatformsInRange(){
		Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
		//print (inRange.Length);
		if (inRange.Length > 0) {
			foreach (Collider2D item in inRange) {
				//print (hello);

				if (item.GetComponent<Rigidbody2D> () && item.CompareTag ("Dwane")) {
					transform.GetComponent<SpriteRenderer> ().enabled = true;
					break;
				} else {
					transform.GetComponent<SpriteRenderer> ().enabled = false;
				}
			
			}
		}
	}
}
