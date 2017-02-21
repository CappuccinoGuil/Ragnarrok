using UnityEngine;
using System.Collections;
using Rewired;

public class playerControllerScript : MonoBehaviour {

	
	[SerializeField] float grabDistance = 0.7f; 
    [SerializeField] Transform holdPoint;
	[SerializeField] float throwforce = 2f;
    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] float canJumpHeight = 0.15f;

    private bool grab = false;
    private RaycastHit2D hit;

    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
    }

	void Update ()
    
     {
		if (player.GetAxis("LHorizontal") > 0)
        {
            Debug.Log(player.GetAxisRaw("LHorizontal"));
			transform.position = new Vector3(transform.position.x + moveSpeed,transform.position.y,0f);
			transform.localScale = new Vector3 (-0.6f, transform.localScale.y, transform.localScale.z);
        }
        if (player.GetAxis("LHorizontal") < 0)
        {
			transform.position = new Vector3(transform.position.x - moveSpeed,transform.position.y,0f);
			transform.localScale = new Vector3 (0.6f, transform.localScale.y, transform.localScale.z);
		}

		if (player.GetButtonDown("AButton") && GroundCheck()) //performs ground check, if player is withing range of ground they can jump
        {
			GetComponent<Rigidbody2D> ().AddForce(Vector2.up * jumpForce);
		}

		//Grabbing and Throwing
		if (player.GetButtonDown("BButton"))
        {
			if (!grab) 
            {
				print ("grab");
				hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.left * transform.localScale.x, grabDistance);
				

				if (hit.collider != null && (hit.collider.gameObject.CompareTag("WoodenObject") || hit.collider.gameObject.CompareTag("PhysicsObject")|| hit.collider.gameObject.CompareTag("Dwane"))) {
					print ("found");
					grab = true;
				}
			} 
            else if (grab) 
            {
				print ("throw");
				grab = false;
				if (hit.collider.gameObject.GetComponent<Rigidbody2D> ()) 
                {
					hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (transform.localScale.x*-2, 1) * throwforce;
				}
			}
		}

		if (grab)
        {
			hit.collider.gameObject.transform.position = holdPoint.position;
		}
	}

    bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position, -transform.up, canJumpHeight); 
    }

}
