﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class playerControllerScript : MonoBehaviour {

	
	[SerializeField] float grabDistance = 1f; 
    [SerializeField] Transform holdPoint;
    [SerializeField] GameObject m_pointer;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float canJumpHeight = 1.1f;
    [SerializeField] float m_throwDistance = 1.0f;
    [SerializeField] float m_rateOfThrowDistIncrease = 2.0f;

    [SerializeField] bool m_useThrow = true;

    public bool m_throwMode = false;

    private bool m_isGrabbing = false;
    private bool m_isAiming = false;
    private bool m_isThrowing = false;
    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_putDown = false;

    private float m_moveSpeed;
    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;

    private RaycastHit2D hit;
    private Quaternion m_tempHoldRotation;
    public List<GameObject> createdAim;

    Rigidbody2D m_rb;
    Rigidbody2D m_rbHit;

    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_rb = GetComponent<Rigidbody2D>();
        m_tempThrowDist = m_throwDistance;
        m_tempHoldRotation = holdPoint.rotation;
    }

	void Update ()
    
     {
        if (!m_throwMode)
        {
            if(player.GetButton("XButton"))
            {
                m_moveSpeed = baseSpeed * 2;
            } else { m_moveSpeed = baseSpeed; }

            if (player.GetAxis("LHorizontal") > 0)
            {
                Debug.Log(player.GetAxisRaw("LHorizontal"));
                transform.position += transform.right * m_moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(-0.6f, transform.localScale.y, transform.localScale.z);
            }
            if (player.GetAxis("LHorizontal") < 0)
            {
                transform.position += transform.right * -m_moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.z);
            }

            if (player.GetButtonDown("AButton") && GroundCheck()) //performs ground check, if player is withing range of ground they can jump
            {
                Jump();
            }
        }
   

        //Grabbing and Throwing
        if (m_useThrow)
        {
            if (player.GetButtonDown("BButton") && !m_isGrabbing)
            {
                print("grab");
                hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.left * transform.localScale.x, grabDistance);
                m_rbHit = hit.rigidbody;

                if (hit && (hit.collider.CompareTag("WoodenObject") || hit.collider.CompareTag("PhysicsObject") || hit.collider.CompareTag("Dwane")))
                {
                    print("found");
                    m_isGrabbing = true;
                }
            }
            if ((player.GetAxisRaw("LHorizontal") != 0 || player.GetAxisRaw("LVertical") != 0) && m_isGrabbing)
            {
                m_tempThrowDist += Time.deltaTime * m_rateOfThrowDistIncrease;

            }
            if ((player.GetAxisRaw("LHorizontal") != 0 || player.GetAxisRaw("LVertical") != 0) && (m_throwMode && !m_isThereAnAimer))
            {
                m_createAimer = true;
                m_isAiming = true;

            }
            else if ((player.GetAxisRaw("LHorizontal") == 0 || player.GetAxisRaw("LVertical") == 0) && m_isThereAnAimer)
            {
                m_yVelocity = m_yVelocity * 0.1f;
                m_xVelocity = m_xVelocity * 0.1f;

                Destroy(createdAim[0]);
                createdAim.Clear();
                m_isThereAnAimer = false;

                m_tempThrowDist = m_throwDistance;
            }
            if (player.GetButtonUp("BButton") && m_isGrabbing && m_isAiming)
            {
                m_isThrowing = true;
                holdPoint.transform.rotation = m_tempHoldRotation;
                m_isAiming = false;
                if (m_isThereAnAimer)
                {
                    Destroy(createdAim[0]);
                    createdAim.Clear();
                    m_isThereAnAimer = false;
                }

            }
            if (player.GetButton("BButton") && m_isGrabbing /*m_isAiming*/)
            {
                m_throwMode = true;
            }
            else
            {
                m_throwMode = false;
                m_tempThrowDist = m_throwDistance;
            }

            if (m_isGrabbing)
            {
                hit.transform.position = holdPoint.position;
            }

            if (m_createAimer)
            {
                CreateAimer();
                m_isThereAnAimer = true;
                m_createAimer = false;
            }

        }

    }

    void FixedUpdate()
    {
        if (m_useThrow)
        {

            if (m_throwMode && m_isThereAnAimer)
            {
                RotateAimer();
                HandleThrow();
            }
            if (m_isThrowing)
            {
                m_rbHit.velocity = new Vector2(m_xVelocity, m_yVelocity);
                m_isThrowing = false;
                m_isGrabbing = false;
            }
        }
           
    }

    void HandleThrow()
    {
        int finalVelocity = 0;
        Vector3 throwDirection = createdAim[0].transform.rotation.eulerAngles;
        float theta = throwDirection.z * Mathf.Deg2Rad;
        float initalVelocity = Mathf.Sqrt(finalVelocity - (2 * Physics2D.gravity.y * m_tempThrowDist));

        m_yVelocity = (initalVelocity * Mathf.Cos(theta));
        m_xVelocity = (initalVelocity * Mathf.Sin(theta)) * -1;

    }

    void RotateAimer()
    {
        float horz = player.GetAxisRaw("LHorizontal");
        float vert = player.GetAxisRaw("LVertical");
        float tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        createdAim[0].transform.rotation = Quaternion.Euler(0, 0, tarAngle - 90);      
    }

    void CreateAimer()
    {
        GameObject createdAimer;
        createdAimer = Instantiate(m_pointer, holdPoint.position + transform.forward * -0.5f, holdPoint.rotation);
        createdAim.Add(createdAimer);
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

}
