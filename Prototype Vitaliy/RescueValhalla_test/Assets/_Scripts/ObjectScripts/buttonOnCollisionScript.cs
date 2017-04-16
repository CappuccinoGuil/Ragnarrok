using UnityEngine;
using System.Collections;

public class buttonOnCollisionScript : MonoBehaviour {
    [Header("External Refrence")]
    [SerializeField] GameObject movingPlatform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if ((coll.gameObject.tag == "WoodenObject"|| coll.gameObject.CompareTag("Dwane")) && movingPlatform !=null)
        {
            movingPlatform.GetComponent<MovingPlatform>().moveSpeed = 1.5f;
			float mirrorScale = transform.localScale.x * -1f;
			transform.localScale = new Vector3(mirrorScale, 1, 1);
        }
			//Destroy(GameObject.FindGameObjectWithTag ("Door"));


	}
}
