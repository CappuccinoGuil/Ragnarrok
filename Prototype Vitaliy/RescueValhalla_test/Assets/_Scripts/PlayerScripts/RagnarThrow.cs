using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class RagnarThrow : MonoBehaviour
{
    [SerializeField] Transform m_holdPoint;
    [SerializeField] Transform m_heldPoint;
    [SerializeField] GameObject m_pointer;
    [SerializeField] float m_grabDistance = 1f;
    [SerializeField] float m_minThrowVelocity = 2.2f;
    [SerializeField] float m_maxThrowDistance = 5f;
    [SerializeField] float m_rateOfThrowDistIncrease = 2.0f;

    [HideInInspector] public bool m_isGrabbing = false;
    [HideInInspector] public bool m_animHoldingObject = false;
    [HideInInspector] public bool m_animNotHoldingObject = false;
    [HideInInspector] public bool m_startPickUpSwitchTimer = false;

    [HideInInspector] public float m_pickUpTimer = 0.3f;

    private bool m_isThrowing = false;
    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_cancelThrow = false;
    private bool m_startCoolDown = false;

    private float m_pickUpCoolDown = 1.0f;
    private float m_ThrowDistance = 1.0f;
    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;
    private float m_axisActiveTime;
    private float m_tempPickUpCoolDown = 0;

    private RaycastHit2D m_hit;

    [HideInInspector] public List<GameObject> createdAim;

    private Rigidbody2D m_rbHit;

    private playerControllerScript m_ragnar;

    //rewired
    private int playerId;
    private Player m_player; // The Rewired Player


    void Awake()
    {
        m_ragnar = gameObject.GetComponent<playerControllerScript>();
        playerId = m_ragnar.playerId;
        m_player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_tempThrowDist = m_ThrowDistance;
    }

    void Update()
    {
        if (m_startCoolDown)
        {
            m_tempPickUpCoolDown -= Time.deltaTime;
            Mathf.Clamp(m_tempPickUpCoolDown, 0, 10);
            if (m_tempPickUpCoolDown <= 0)
            {
                m_tempPickUpCoolDown = 0;
                m_startCoolDown = false;
            }
        }

        if (m_startPickUpSwitchTimer)
        {
            PickUpTimer();
        }

        //Pick up the object.
        if (m_player.GetButtonDown("RTrigger") && !m_isGrabbing && m_tempPickUpCoolDown == 0)
        {
            print("grab");
            m_hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.right * transform.localScale.x, m_grabDistance);
            m_rbHit = m_hit.rigidbody;

            if (m_hit && (m_hit.collider.CompareTag("WoodenObject") || m_hit.collider.CompareTag("PhysicsObject") || m_hit.collider.CompareTag("InteractiveBox") || m_hit.collider.CompareTag("Dwane")))
            {
                m_tempPickUpCoolDown = m_pickUpCoolDown;
                m_startCoolDown = true;
                m_ragnar.myAnim.SetTrigger("startPickUp");

                print("found");
                m_isGrabbing = true;
                m_animNotHoldingObject = true;
                m_hit.collider.enabled = false;
            }

        }
        //Put down the object.
        else if (m_player.GetButtonDown("RTrigger") && m_isGrabbing && !m_isThereAnAimer)
        {
            if (m_ragnar.m_facingRight)
            {
                m_yVelocity = m_minThrowVelocity + (m_minThrowVelocity * 0.4f);
                m_xVelocity = m_minThrowVelocity;
            }
            if (!m_ragnar.m_facingRight)
            {
                m_yVelocity = m_minThrowVelocity + (m_minThrowVelocity * 0.4f);
                m_xVelocity = m_minThrowVelocity * - 1;
            }

            m_isThrowing = true;
            m_ragnar.myAnim.SetTrigger("isThrowing");
        }
        //Cancel the throw.
        else if (m_player.GetButtonDown("RTrigger") && m_isThereAnAimer)
        {
            m_cancelThrow = true;

            m_ragnar.myAnim.SetBool("isCharging", false);
            m_ragnar.myAnim.SetBool("cancelThrow", true);

            Destroy(createdAim[0]);
            createdAim.Clear();
            m_ragnar.m_throwMode = false;
            m_tempThrowDist = m_ThrowDistance;

            m_isThereAnAimer = false;

        }
        //Set cancel throw to false when joy stick has been inactive for a moment.
        if ((m_player.GetAxisRawTimeInactive("RHorizontal") > 0.1f || m_player.GetAxisRawTimeInactive("RVertical") > 0.1f))
        {
            m_cancelThrow = false;
        }

        if (!m_cancelThrow)
        {
            //Flip Ragnar around.
            if (m_player.GetAxis("RHorizontal") > 0)
            {
                if (m_ragnar.m_facingRight)
                {
                    m_ragnar.Flip();
                }
            }
            if (m_player.GetAxis("RHorizontal") < 0)
            {
                if (!m_ragnar.m_facingRight)
                {
                    m_ragnar.Flip();
                }
            }
            //Increase throw power while holding Right joy stick.
            if ((m_player.GetAxisRaw("RHorizontal") != 0 || m_player.GetAxisRaw("RVertical") != 0) && m_isGrabbing)
            {
                m_tempThrowDist += Time.deltaTime * m_rateOfThrowDistIncrease;
                m_tempThrowDist = Mathf.Clamp(m_tempThrowDist, 0f, m_maxThrowDistance);
            }
            //Set create aimer if Right joy stick is held and there is no aimer.
            if ((m_player.GetAxisRaw("RHorizontal") != 0.0f || m_player.GetAxisRaw("RVertical") != 0.0f) && (m_isGrabbing && !m_isThereAnAimer))
            {
                m_ragnar.m_throwMode = true;
                m_createAimer = true;
            }
            //Launch the object if player releases the Right joy stick.
            else if ((m_player.GetAxisRaw("RHorizontal") == 0 && m_player.GetAxisRaw("RVertical") == 0) && m_isThereAnAimer)
            {
                m_isThrowing = true;

                m_ragnar.myAnim.SetTrigger("isThrowing");

                Destroy(createdAim[0]);
                createdAim.Clear();
                m_ragnar.m_throwMode = false;
                m_tempThrowDist = m_ThrowDistance;

                m_isThereAnAimer = false;
            }
        }

        //Switching the position of the held object based on animation state.
        if (m_isGrabbing)
        {
            if (m_animNotHoldingObject)
            {
                m_hit.transform.position = m_holdPoint.position;
                m_hit.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (m_animHoldingObject)
            {
                m_animNotHoldingObject = false;
                m_hit.transform.position = m_heldPoint.position;
            }
        }
        

        //Create the aimer.
        if (m_createAimer)
        {
            m_ragnar.myAnim.SetBool("isCharging", true);
            CreateAimer();
            m_isThereAnAimer = true;
            m_createAimer = false;
        }
    }

    void FixedUpdate()
    {
        if (m_ragnar.m_throwMode && m_isThereAnAimer)
        {
            RotateAimer();
            HandleThrow();
            
        }
        if (m_isThrowing)
        {
            m_ragnar.myAnim.SetBool("isCharging", false);

            m_rbHit.velocity = new Vector2(m_xVelocity, m_yVelocity);
            StartCoroutine(WaitToActivateCollision());
            m_isThrowing = false;
            m_animHoldingObject = false;
            m_isGrabbing = false;
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
        float horz = m_player.GetAxisRaw("RHorizontal");
        float vert = m_player.GetAxisRaw("RVertical");
        float tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        createdAim[0].transform.rotation = Quaternion.Euler(0, 0, tarAngle + 90);
        createdAim[0].transform.position = m_heldPoint.position + transform.forward * -0.5f;
    }

    void CreateAimer()
    {
        GameObject createdAimer;
        createdAimer = Instantiate(m_pointer, m_heldPoint.position + transform.forward * -0.5f, m_heldPoint.rotation);
        createdAim.Add(createdAimer);
    }

    void PickUpTimer()
    {
        m_pickUpTimer -= Time.deltaTime;
        if(m_pickUpTimer <= 0)
        {
            m_animHoldingObject = true;
            m_startPickUpSwitchTimer = false;
        }
    }

    IEnumerator WaitToActivateCollision()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        m_hit.collider.enabled = true;
        m_hit.rigidbody.constraints = RigidbodyConstraints2D.None;
    }

}