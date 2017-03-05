using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testscript : MonoBehaviour {

	//GameObject [] PhysicsObjectsOnLevel;
	List <GameObject> PhysicsObjects;
	public GameObject Dwane;
	float impulsePower = 15f;
	float dragPower = 0.4f;
	float effectRadius = 1.8f;

	//for figuring out is it push or hold
	float holdTime;
	float startingHoldTime = 0.25f;
	float triggerHoldTime = 0;

	// Use this for initialization
	void Start () {
		//PhysicsObjects = GameObject.FindGameObjectsWithTag ("PhysicsObject");

		//giving a starting value to starting HoldTime
		holdTime = startingHoldTime;
	}
	
	// Update is called once per frame
	void Update () {

		InputDetector ();

	}

	void InputDetector(){
		//listenning to left mouse button
		//PUSH
		if (Input.GetKeyUp(KeyCode.W) && holdTime >= triggerHoldTime) {
			print ("a push");

			ApplyForceOut ("push");

			holdTime = startingHoldTime;
		} else if (Input.GetKeyUp(KeyCode.W) && holdTime < triggerHoldTime) {
			print ("renewed");
			holdTime = startingHoldTime;
		}

		//HOLD
		if (Input.GetKey (KeyCode.W)) {
			holdTime -= Time.deltaTime;

			if(holdTime<triggerHoldTime)

				ApplyForceOut ("hold");

				print ("is holding");
		}

		//listenning to right mouse button
		//PUSH
		if (Input.GetKeyUp (KeyCode.S) && holdTime >= triggerHoldTime) {
			print ("a push");

			ApplyForceIn ("push");

			holdTime = startingHoldTime;
		} else if (Input.GetKeyUp (KeyCode.S) && holdTime < triggerHoldTime) {
			print ("renewed");
			holdTime = startingHoldTime;
		}

		//HOLD
		if (Input.GetKey (KeyCode.S)) {
			holdTime -= Time.deltaTime;

			if(holdTime<triggerHoldTime)

				ApplyForceIn ("hold");

				print ("is holding");
		}
	}

	List<GameObject> GetPhysicsObjectsInRadius(){
		
		Collider2D[] inRange = Physics2D.OverlapCircleAll(Dwane.transform.position, effectRadius);
		List<GameObject>tempInRadius = new List<GameObject>();

		for (int i = 0; i < inRange.Length; i++) {
			if (inRange[i].gameObject.CompareTag("PhysicsObject")||inRange[i].gameObject.CompareTag("IronWall"))
			{
				tempInRadius.Add(inRange[i].gameObject); ;
			}
		}

		return tempInRadius;
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(Dwane.transform.position, effectRadius);
	}

	void ApplyForceOut(string inputType){
		Vector2 mousepositionTemp = Input.mousePosition;
		Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		PhysicsObjects = GetPhysicsObjectsInRadius ();

		if (PhysicsObjects.Count < 1) {
			print ("empty object array");
			return;
		}
			
		///////////// for Dwane

		for(int i = 0;i<PhysicsObjects.Count;i++){

			if(PhysicsObjects[i].CompareTag("IronWall")){
				if (PhysicsObjects [i].transform.position.x >= Dwane.transform.position.x) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.left * dragPower, ForceMode2D.Impulse);
					}
					else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.left * impulsePower, ForceMode2D.Impulse);
					}
				} else if (PhysicsObjects [i].transform.position.x < Dwane.transform.position.x) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * dragPower, ForceMode2D.Impulse);
					}
					else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * impulsePower, ForceMode2D.Impulse);
					}
				}

				if (PhysicsObjects [i].transform.position.y < Dwane.transform.position.y) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * dragPower, ForceMode2D.Impulse);
					}
					else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * impulsePower, ForceMode2D.Impulse);
					}
				}else if(PhysicsObjects [i].transform.position.y >= Dwane.transform.position.y){
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * dragPower, ForceMode2D.Impulse);
					}
					else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * impulsePower, ForceMode2D.Impulse);
					}
				}
			}

			///////////// for Physics Objects

			if (PhysicsObjects [i].transform.position.x < Dwane.transform.position.x) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.left * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.left * impulsePower, ForceMode2D.Impulse);
				}
			} else if (PhysicsObjects [i].transform.position.x >= Dwane.transform.position.x) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.right * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.right * impulsePower, ForceMode2D.Impulse);
				}
			}


			if (PhysicsObjects [i].transform.position.y > Dwane.transform.position.y) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.up * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.up * impulsePower, ForceMode2D.Impulse);
				}
			} else if (PhysicsObjects [i].transform.position.y <= Dwane.transform.position.y) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.down * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.down * impulsePower, ForceMode2D.Impulse);
				}
			}

		}
	}


	void ApplyForceIn(string inputType){
		Vector2 mousepositionTemp = Input.mousePosition;
		Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		PhysicsObjects = GetPhysicsObjectsInRadius ();

		if (PhysicsObjects.Count < 1) {
			return;
		}

		///////////// for Dwane

		for (int i = 0; i < PhysicsObjects.Count; i++) {

			if (PhysicsObjects [i].CompareTag ("IronWall")) {
				if (PhysicsObjects [i].transform.position.x < Dwane.transform.position.x) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.left * dragPower, ForceMode2D.Impulse);
					} else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.left * impulsePower, ForceMode2D.Impulse);
					}
				} else if (PhysicsObjects [i].transform.position.x >= Dwane.transform.position.x) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * dragPower, ForceMode2D.Impulse);
					} else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * impulsePower, ForceMode2D.Impulse);
					}
				}

				if (PhysicsObjects [i].transform.position.y >= Dwane.transform.position.y) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * dragPower, ForceMode2D.Impulse);
					} else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.up * impulsePower, ForceMode2D.Impulse);
					}
				} else if (PhysicsObjects [i].transform.position.y < Dwane.transform.position.y) {
					if (inputType == "hold") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * dragPower, ForceMode2D.Impulse);
					} else if (inputType == "push") {
						Dwane.GetComponent<Rigidbody2D> ().AddForce (Vector2.down * impulsePower, ForceMode2D.Impulse);
					}
				}
			}
		}

		//For Physics Objects
		for(int i = 0;i<PhysicsObjects.Count;i++){

			if (PhysicsObjects [i].transform.position.x < Dwane.transform.position.x) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.right * dragPower, ForceMode2D.Impulse);
				} else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.right * impulsePower, ForceMode2D.Impulse);
				}
			} else if (PhysicsObjects [i].transform.position.x >= Dwane.transform.position.x) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.left * dragPower, ForceMode2D.Impulse);
				} else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.left * impulsePower, ForceMode2D.Impulse);
				}
			}

			if (PhysicsObjects [i].transform.position.y > Dwane.transform.position.y) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.down * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.down * impulsePower, ForceMode2D.Impulse);
				}
			} else if (PhysicsObjects [i].transform.position.y <= Dwane.transform.position.y) {
				if (inputType == "hold") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.up * dragPower, ForceMode2D.Impulse);
				}
				else if (inputType == "push") {
					PhysicsObjects [i].GetComponent<Rigidbody2D> ().AddForce (Vector2.up * impulsePower, ForceMode2D.Impulse);
				}
			}
		}
	}
}
