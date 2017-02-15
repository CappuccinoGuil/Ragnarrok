using UnityEngine;
using System.Collections;

public class playerControllerScript : MonoBehaviour {

	bool grab = false;
	RaycastHit2D hit;
	float distance = 0.7f;
	public Transform holdpoint;
	public float throwforce = 2f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow)){
			this.transform.position = new Vector3(this.transform.position.x + 0.1f,this.transform.position.y,0f);
			this.transform.localScale = new Vector3 (-0.6f, this.transform.localScale.y, this.transform.localScale.z);
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			this.transform.position = new Vector3(this.transform.position.x - 0.1f,this.transform.position.y,0f);
			this.transform.localScale = new Vector3 (0.6f, this.transform.localScale.y, this.transform.localScale.z);
		}

		if(Input.GetKeyDown(KeyCode.UpArrow)){
			this.GetComponent<Rigidbody2D> ().AddForce(Vector2.up * 300f);
		}

		//Grabbing and Throwing
		if(Input.GetKeyDown(KeyCode.M)){
			if (!grab) {
				print ("grab");
				Physics2D.queriesStartInColliders = false;
				hit = Physics2D.Raycast (this.transform.position, Vector2.left * this.transform.localScale.x, distance);
				print (this.transform.localScale.x);

				if (hit.collider != null && (hit.collider.gameObject.CompareTag("WoodenObject") || hit.collider.gameObject.CompareTag("PhysicsObject")|| hit.collider.gameObject.CompareTag("Dwane"))) {
					print ("found");
					grab = true;

				}
			} else if (grab) {
				print ("throw");
				grab = false;
				if (hit.collider.gameObject.GetComponent<Rigidbody2D> () != null) {
					hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (this.transform.localScale.x*-2, 1) * throwforce;
					//hit.collider.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (this.transform.localScale.x*-2, 1) * throwforce, ForceMode2D.Impulse);
				}
			}
		}

		if (grab) {
			//print ("hold");
			hit.collider.gameObject.transform.position = holdpoint.position;
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawLine (this.transform.position, this.transform.position + Vector3.left *distance);
	}
}
