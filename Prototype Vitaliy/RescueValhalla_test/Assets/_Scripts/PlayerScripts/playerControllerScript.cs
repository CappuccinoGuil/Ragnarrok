using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class playerControllerScript : MonoBehaviour {

    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float canJumpHeight = 1.4f;

    public bool m_throwMode = false;
    public bool m_facingRight = true;

    private float m_moveSpeed;
    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;

    private RaycastHit2D hit;
    private Quaternion m_tempHoldRotation;
    public List<GameObject> createdAim;

    Rigidbody2D m_rb;
    Rigidbody2D m_rbHit;

    RagnarThrow throwScript;

    public Animator myAnim;

    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    void Start()
    {
        throwScript = gameObject.GetComponent<RagnarThrow>();
    }

    void Update()

    {
        if (!m_throwMode && !throwScript.m_isGrabbing)
        {
            if (player.GetButton("XButton"))
            {
                m_moveSpeed = baseSpeed * 2;
            }
            else { m_moveSpeed = baseSpeed; }

            if (player.GetAxis("LHorizontal") > 0)
            {
                transform.position += transform.right * m_moveSpeed * Time.deltaTime;
                if(!m_facingRight)
                {
                    Flip();
                }
            }           
            if (player.GetAxis("LHorizontal") < 0)
            {
                transform.position += transform.right * -m_moveSpeed * Time.deltaTime;
                if(m_facingRight)
                {
                    Flip();
                }
            }
            if (player.GetButtonDown("AButton") && GroundCheck()) //performs ground check, if player is withing range of ground they can jump
            {
                Jump();
            }
        }
        if(throwScript.m_isGrabbing && !m_throwMode)
        {
            if (player.GetAxis("LHorizontal") > 0 && throwScript.m_isGrabbing)
            {
                transform.position += transform.right * (m_moveSpeed * 0.5f) * Time.deltaTime;
                if (!m_facingRight)
                {
                    Flip();
                }
            }

            if (player.GetAxis("LHorizontal") < 0 && throwScript.m_isGrabbing)
            {
                transform.position += transform.right * (-m_moveSpeed * 0.5f) * Time.deltaTime;
                if (m_facingRight)
                {
                    Flip();
                }
            }
        }
        if (m_throwMode && throwScript.m_isGrabbing)
        {
            if (player.GetAxis("LHorizontal") > 0)
            {
                transform.position += transform.right * (m_moveSpeed * 0.25f) * Time.deltaTime;
            }
            if (player.GetAxis("LHorizontal") < 0)
            {
                transform.position += transform.right * (-m_moveSpeed * 0.25f) * Time.deltaTime;
            }
        }
    }
    
    void FixedUpdate()
    {
        myAnim.SetFloat("verticalSpeed", m_rb.velocity.y);
        myAnim.SetBool("isGrounded", GroundCheck());
        myAnim.SetFloat("speed", Mathf.Abs(player.GetAxis("LHorizontal")));
    } 

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight);
        float jumpAcceleration = (0 - jumpVelocity) / (0 - 1);
        float force = m_rb.mass * jumpAcceleration;

        m_rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position, -transform.up, canJumpHeight); 
    }

    public void Flip()
    {
        m_facingRight = !m_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
