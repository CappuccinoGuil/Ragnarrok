using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class playerControllerScript : MonoBehaviour {

    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float canJumpHeight = 1.4f;
    [SerializeField] float m_fallMultiplier = 2.5f;
    [SerializeField] float m_lowJumpMultiplier = 2.0f;

    [HideInInspector] public bool m_throwMode = false;
    [HideInInspector] public bool m_blastMode = false;
    [HideInInspector] public bool m_facingRight = true;

    private float m_moveSpeed;
    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;

    private RaycastHit2D hit;
    private Quaternion m_tempHoldRotation;
    [HideInInspector] public List<GameObject> createdAim;

    Rigidbody2D m_rb;
    Rigidbody2D m_rbHit;

    RagnarThrow throwScript;

    [HideInInspector] public Animator myAnim;

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
        if (!m_throwMode && !throwScript.m_isGrabbing && !m_blastMode)
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
        if(throwScript.m_isGrabbing && !m_throwMode && !m_blastMode)
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
        if (m_throwMode && throwScript.m_isGrabbing && !m_blastMode)
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
        if (m_blastMode)
        {
            if (player.GetAxis("LHorizontal") > 0)
            {
                transform.position += transform.right * (m_moveSpeed) * Time.deltaTime;
            }
            if (player.GetAxis("LHorizontal") < 0)
            {
                transform.position += transform.right * (-m_moveSpeed) * Time.deltaTime;
            }
        }
    }
    
    void FixedUpdate()
    {
        myAnim.SetFloat("verticalSpeed", m_rb.velocity.y);
        myAnim.SetBool("isGrounded", GroundCheck());
        myAnim.SetFloat("speed", Mathf.Abs(player.GetAxis("LHorizontal")));

        if (m_rb.velocity.y < 0)
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (m_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (m_rb.velocity.y > 0 && !player.GetButton("AButton"))
        {
            m_rb.velocity += Vector2.up * Physics2D.gravity.y * (m_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight);
        float jumpAcceleration = (0 - jumpVelocity) / (0 - 1);
        float force = m_rb.mass * jumpAcceleration;

        m_rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public bool GroundCheck()
    {
        return (Physics2D.Raycast(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), -transform.up, canJumpHeight)) || 
            (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.up, canJumpHeight)) ||
            (Physics2D.Raycast(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), -transform.up, canJumpHeight));
    }

    public void Flip()
    {
        m_facingRight = !m_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
