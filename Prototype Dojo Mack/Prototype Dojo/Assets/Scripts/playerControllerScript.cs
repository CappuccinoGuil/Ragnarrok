using UnityEngine;
using System.Collections;

public class playerControllerScript : MonoBehaviour {

	bool grab = false;
	RaycastHit2D hit;
	float distance = 0.7f;
	public Transform holdpoint;
	public float throwforce = 2f;

    private bool m_isGrounded;

    private Rigidbody2D m_rb;

  


    // Use this for initialization
    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position = new Vector3(transform.position.x + 0.1f,transform.position.y,0f);
			transform.localScale = new Vector3 (-0.6f, transform.localScale.y, transform.localScale.z);
           // transform.localRotation = new Quaternion()
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position = new Vector3(transform.position.x - 0.1f,transform.position.y,0f);
			transform.localScale = new Vector3 (0.6f, transform.localScale.y, transform.localScale.z);
		}

		if(Input.GetKeyDown(KeyCode.UpArrow) && m_isGrounded){
            m_rb.AddForce(Vector2.up * 300f);
		}

		//Grabbing and Throwing
		if(Input.GetKeyDown(KeyCode.M)){
			if (!grab) {
				print ("grab");
				Physics2D.queriesStartInColliders = false;
				hit = Physics2D.Raycast (transform.position, Vector2.left * transform.localScale.x, distance);
				print (transform.localScale.x);

				if (hit.collider != null && (hit.collider.gameObject.CompareTag("WoodenObject") || hit.collider.gameObject.CompareTag("PhysicsObject")|| hit.collider.gameObject.CompareTag("Dwane"))) {
					print ("found");
					grab = true;

				}
			} else if (grab) {
				print ("throw");
				grab = false;
				if (hit.collider.gameObject.GetComponent<Rigidbody2D> () != null) {
					hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (transform.localScale.x*-2, 1) * throwforce;
					//hit.collider.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (transform.localScale.x*-2, 1) * throwforce, ForceMode2D.Impulse);
				}
			}
		}

		if (grab) {
			//print ("hold");
			hit.collider.gameObject.transform.position = holdpoint.position;
		}
	}


    void OnCollisionEnter2D(Collision2D col)
    {
        m_isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        m_isGrounded = false;
    }


    void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawLine (transform.position, transform.position + Vector3.left *distance);
	}

}
