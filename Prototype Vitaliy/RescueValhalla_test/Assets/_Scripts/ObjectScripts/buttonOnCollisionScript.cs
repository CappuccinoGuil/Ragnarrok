﻿using UnityEngine;
using System.Collections;

public class buttonOnCollisionScript : MonoBehaviour {
    [Header("External Refrence")]
    [SerializeField] GameObject movingPlatform;
    [SerializeField] GameObject[] walls;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if ((coll.gameObject.tag == "WoodenObject"|| coll.gameObject.CompareTag("InteractiveBox") || coll.gameObject.CompareTag("Dwane") || coll.gameObject.CompareTag("Ragnar")) && movingPlatform !=null)
        {

			float mirrorScale = transform.localScale.x * -1f;
			transform.localScale = new Vector3(mirrorScale, transform.localScale.y, transform.localScale.z);

            movingPlatform.GetComponent<MovingPlatform>().moveSpeed = 1.5f;
            if (walls !=null) //turns on invisible walls so the player doesnt fall
            {
                foreach (GameObject wall in walls)
                {
                    wall.SetActive(true);
                }
            }
        }
			//Destroy(GameObject.FindGameObjectWithTag ("Door"));
	}
}
