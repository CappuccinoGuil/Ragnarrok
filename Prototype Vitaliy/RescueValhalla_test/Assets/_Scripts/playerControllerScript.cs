using UnityEngine;
using System.Collections;
using Rewired;

public class playerControllerScript : MonoBehaviour {

	
	[SerializeField] float grabDistance = 0.7f; 
    [SerializeField] Transform holdPoint;
    [SerializeField] Transform m_pointer;
    [SerializeField] float moveSpeed = 0.05f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float canJumpHeight = 0.15f;
    [SerializeField] float m_throwDistance = 3.0f;
    private float m_tempThrowDist;
    private Quaternion m_tempHoldRotation;

    private bool m_isGrabbing = false;
    private bool m_isAiming = false;
    private bool m_isThrowing = false;
    
    public float m_yVelocity;
    public float m_xVelocity;

    private RaycastHit2D hit;

    Rigidbody2D m_rb;
    Rigidbody2D m_rbHit;

    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player

    public float checkJump;


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_rb = GetComponent<Rigidbody2D>();
        m_tempThrowDist = m_throwDistance;
        m_tempHoldRotation = holdPoint.rotation;
    }

	void Update ()
    
     {
        if (!m_isAiming)
        {
            if (player.GetAxis("LHorizontal") > 0)
            {
                Debug.Log(player.GetAxisRaw("LHorizontal"));
                transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, 0f);
                transform.localScale = new Vector3(-0.6f, transform.localScale.y, transform.localScale.z);
            }
            if (player.GetAxis("LHorizontal") < 0)
            {
                transform.position = new Vector3(transform.position.x - moveSpeed, transform.position.y, 0f);
                transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.z);
            }

            if (player.GetButtonDown("AButton") && GroundCheck()) //performs ground check, if player is withing range of ground they can jump
            {
                Jump();
            }
        }

		//Grabbing and Throwing
		if (player.GetButtonUp("BButton"))
        {
			if (!m_isGrabbing) 
            {
				print ("grab");
				hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.left * transform.localScale.x, grabDistance);
                m_rbHit = hit.rigidbody;

                if (hit.collider != null && (hit.collider.gameObject.CompareTag("WoodenObject") || hit.collider.gameObject.CompareTag("PhysicsObject")|| hit.collider.gameObject.CompareTag("Dwane"))) {
					print ("found");
					m_isGrabbing = true;
				}
			} 
            else if (m_isGrabbing) 
            {
				print ("throw");
				m_isGrabbing = false;
				if (hit) 
                {
                    m_isThrowing = true;

                    // setting hold point rotation back to origin after throw
                    holdPoint.transform.rotation = m_tempHoldRotation;
                    
                    //hit.collider.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (transform.localScale.x*-2, 1) * throwforce;
                }
            }
		} else if(player.GetButton("BButton") && m_isAiming)
        {
            m_tempThrowDist += Time.deltaTime * 1;
        } else { m_tempThrowDist = m_throwDistance; }

		if (m_isGrabbing)
        {
			hit.collider.gameObject.transform.position = holdPoint.position;
		}

        if (player.GetButton("LTrigger") && m_isGrabbing)
        {
            m_isAiming = true;
        } else { m_isAiming = false; }

        if(m_isAiming)
        {

           // RotatePointer();
            if (player.GetAxis("LVertical") > 0)
            {
                holdPoint.Rotate(Vector3.back * Time.deltaTime * 15);
            }
            if (player.GetAxis("LVertical") < 0)
            {
                holdPoint.Rotate(Vector3.forward * Time.deltaTime * 15);
            }
        }
    }

    void FixedUpdate()
    {
        if(m_isAiming)
        {
          HandleThrow();
        }
        if(m_isThrowing)
        {
         //    m_rbHit.AddForce(m_force, ForceMode2D.Impulse);
         m_rbHit.velocity = new Vector2(m_xVelocity, m_yVelocity);
          m_isThrowing = false;
        }
    }

    void HandleThrow()
    {
         float objectForward = transform.localScale.x;

        int finalVelocity = 0;

        Vector3 throwDirection = holdPoint.rotation.eulerAngles;
        float theta = throwDirection.z * Mathf.Deg2Rad;

        //float initalVelocity = Mathf.Sqrt(finalVelocity - (2 * Physics2D.gravity.y * throwDist));
        ////float initialVelocityX = (2 * (m_throwDistance / m_timeToThrowDistance)) - finalVelocity;

        //m_yVelocity = initalVelocity * Mathf.Sin(theta);
        //m_xVelocity = initalVelocity * Mathf.Cos(theta);

        //float time = (m_throwDistance) / (0.5f * Physics2D.gravity.y);

        //float accelerationY = 2 * (m_throwDistance - (m_yVelocity * time)) / (Mathf.Pow(time, 2));
        //float accelerationX = 2 * (m_throwDistance - (m_xVelocity * time)) / (Mathf.Pow(time, 2));

        //m_force.y = hit.rigidbody.mass * accelerationY;
        //m_force.x = hit.rigidbody.mass * accelerationX;

        float initalVelocity = Mathf.Sqrt(finalVelocity - (2 * Physics2D.gravity.y * m_tempThrowDist));

        m_yVelocity = initalVelocity * Mathf.Sin(theta);
        m_xVelocity = initalVelocity * Mathf.Cos(theta);


        //float totalVelocity = throwDist / Mathf.Cos(theta);

        if (objectForward > 0)
        {
            m_yVelocity = (initalVelocity * Mathf.Sin(theta)) * -1;
            m_xVelocity = (initalVelocity * Mathf.Cos(theta)) * -1;
        }
        if (objectForward < 0)
        {
            m_yVelocity = initalVelocity * Mathf.Sin(theta);
            m_xVelocity = initalVelocity * Mathf.Cos(theta);
        }

    }

    void RotatePointer()
    {
        float horz = player.GetAxisRaw("LHorizontal");
        float vert = player.GetAxisRaw("LVertical");
        float tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        m_pointer.rotation = Quaternion.Euler(0, 0, tarAngle - 90);

        
    }

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight);
        checkJump = jumpVelocity;
        m_rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
    }

    bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position, -transform.up, canJumpHeight); 
    }

}
