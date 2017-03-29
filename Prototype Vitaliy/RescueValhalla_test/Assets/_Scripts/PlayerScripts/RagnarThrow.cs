using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class RagnarThrow : MonoBehaviour
{

    [SerializeField] float m_grabDistance = 1f;
    [SerializeField] Transform m_holdPoint;
    [SerializeField] Transform m_heldPoint;
    [SerializeField] GameObject m_pointer;
    [SerializeField] float m_throwDistance = 1.0f;
    [SerializeField] float m_rateOfThrowDistIncrease = 2.0f;

    [HideInInspector] public bool m_isGrabbing = false;
    [SerializeField] public bool m_animHoldingObject = false;
    [SerializeField] public bool m_animNotHoldingObject = false;

    private bool m_isThrowing = false;
    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_putDown = false;
    private bool m_cancelThrow = false;

    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;
    private float m_axisActiveTime;

    private RaycastHit2D m_hit;
    private Quaternion m_tempHoldRotation;

    [HideInInspector] public List<GameObject> createdAim;

    private Rigidbody2D m_rbHit;

    private playerControllerScript m_ragnar;

    //rewired
    public int playerId = 0;
    private Player m_player; // The Rewired Player


    void Awake()
    {
        m_player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_tempThrowDist = m_throwDistance;
    }

    void Start()
    {
        m_ragnar = gameObject.GetComponent<playerControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_player.GetButtonDown("RTrigger") && !m_isGrabbing)
        {
            print("grab");
            m_hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.right * transform.localScale.x, m_grabDistance);
            m_rbHit = m_hit.rigidbody;

            if (m_hit && (m_hit.collider.CompareTag("WoodenObject") || m_hit.collider.CompareTag("PhysicsObject") || m_hit.collider.CompareTag("Dwane")))
            {
                m_ragnar.myAnim.SetBool("pickUp", true);

                print("found");
                m_isGrabbing = true;
                m_animNotHoldingObject = true;
                m_hit.collider.enabled = false;
            }

        }
        else if (m_player.GetButtonDown("RTrigger") && m_isGrabbing && !m_isThereAnAimer)
        {
            if (m_ragnar.m_facingRight)
            {
                m_yVelocity = 2.5f;
                m_xVelocity = 2.5f;
            }
            if (!m_ragnar.m_facingRight)
            {
                m_yVelocity = 2.5f;
                m_xVelocity = 2.5f * -1;
            }

            m_isThrowing = true;
            m_ragnar.myAnim.SetTrigger("isThrowing");
        }
        else if (m_player.GetButtonDown("RTrigger") && m_isThereAnAimer)
        {
            m_cancelThrow = true;
            m_heldPoint.transform.rotation = m_tempHoldRotation;

            m_ragnar.myAnim.SetBool("isCharging", false);
            m_ragnar.myAnim.SetBool("pickUp", true);

            Destroy(createdAim[0]);
            createdAim.Clear();
            m_ragnar.m_throwMode = false;
            m_tempThrowDist = m_throwDistance;

            m_isThereAnAimer = false;

        }

        if ((m_player.GetAxisRawTimeInactive("RHorizontal") > 0.1f || m_player.GetAxisRawTimeInactive("RVertical") > 0.1f))
        {
            m_cancelThrow = false;
        }

        if (!m_cancelThrow)
        {
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
            if ((m_player.GetAxisRaw("RHorizontal") != 0 || m_player.GetAxisRaw("RVertical") != 0) && m_isGrabbing)
            {
                m_tempThrowDist += Time.deltaTime * m_rateOfThrowDistIncrease;
            }
            if ((m_player.GetAxisRaw("RHorizontal") != 0.0f || m_player.GetAxisRaw("RVertical") != 0.0f) && (m_isGrabbing && !m_isThereAnAimer))
            {
                m_ragnar.m_throwMode = true;
                m_createAimer = true;
            }
            else if ((m_player.GetAxisRaw("RHorizontal") == 0 && m_player.GetAxisRaw("RVertical") == 0) && m_isThereAnAimer)
            {
                m_isThrowing = true;
                m_holdPoint.transform.rotation = m_tempHoldRotation;

                m_ragnar.myAnim.SetTrigger("isThrowing");

                Destroy(createdAim[0]);
                createdAim.Clear();
                m_ragnar.m_throwMode = false;
                m_tempThrowDist = m_throwDistance;

                m_isThereAnAimer = false;
            }
        }

        if (m_isGrabbing)
        {
            if (m_animNotHoldingObject)
            {
                m_hit.transform.position = m_holdPoint.position;
            }
            if (m_animHoldingObject)
            {
                m_animNotHoldingObject = false;
                m_hit.transform.position = m_heldPoint.position;
            }
        }
        


        if (m_createAimer)
        {
            m_ragnar.myAnim.SetBool("pickUp", false);
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
            m_ragnar.myAnim.SetBool("pickUp", false);
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

    IEnumerator WaitToActivateCollision()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        m_hit.collider.enabled = true;
    }

}