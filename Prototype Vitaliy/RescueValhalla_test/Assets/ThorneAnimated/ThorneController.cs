using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorneController : MonoBehaviour {

	//movment variaables
	public float maxSpeed;
	Rigidbody2D myRB;
	public Animator myAnim;
	bool facingRight;
	bool itemHeld = false;

	//jumping variables

	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;

	// Use this for initialization
	void Start () {
		facingRight = true;
		myRB = GetComponent<Rigidbody2D> ();
		//myAnim = GetComponentsInChildren<Animator> ();
		print (myAnim);
	}
	
	// Update is called once per frame
	void Update (){
		if (grounded && Input.GetAxis ("Jump") > 0) {
			grounded = false;
			myAnim.SetBool ("isGrounded", grounded);
			myRB.AddForce (new Vector2 (0, jumpHeight));
		}
		if (itemHeld) {

			if (Input.GetButtonDown("Fire1")) {
				myAnim.SetBool ("pickUp",false);
				myAnim.SetBool ("isCharging",true);
				//myAnim.SetTrigger ("isThrowing");
			} else if (Input.GetButtonUp("Fire1")){
				myAnim.SetBool ("isCharging",false);
				myAnim.SetTrigger ("isThrowing");
				itemHeld = false;
			}

		}else if (!itemHeld) {

			if (Input.GetButtonDown ("Fire1")) {
				myAnim.SetBool ("pickUp", true);
				myAnim.SetBool ("isCharging", false);

			} else if (Input.GetButtonUp ("Fire1")) {
				itemHeld = true;
			}

		}
	}


	void FixedUpdate () {

		//check if we are grounded
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
		myAnim.SetBool ("isGrounded", grounded);
		myAnim.SetFloat ("verticalSpeed", myRB.velocity.y);

		float move = Input.GetAxis ("Horizontal");
		myRB.velocity = new Vector2 (move * maxSpeed, myRB.velocity.y);
		myAnim.SetFloat ("speed", Mathf.Abs (move));

		if (move > 0 && !facingRight) {
			Flip ();
		} else if(move<0 &&facingRight){
			Flip ();
		}
	}

	void Flip (){

		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
