using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class RagnarThrow : MonoBehaviour
{

    [SerializeField] float grabDistance = 1f;
    [SerializeField] Transform holdPoint;
    [SerializeField] GameObject m_pointer;
    [SerializeField] float m_throwDistance = 1.0f;
    [SerializeField] float m_rateOfThrowDistIncrease = 2.0f;

    public bool m_isGrabbing = false;

    private bool m_isThrowing = false;
    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_putDown = false;
    private bool m_cancelThrow = false;

    private float m_yVelocity;
    private float m_xVelocity;
    private float m_tempThrowDist;
    private float m_axisActiveTime;

    private RaycastHit2D hit;
    private Quaternion m_tempHoldRotation;
    public List<GameObject> createdAim;

    Rigidbody2D m_rbHit;

    playerControllerScript ragnar;

    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
        m_tempThrowDist = m_throwDistance;
        m_tempHoldRotation = holdPoint.rotation;
    }

    void Start()
    {
        ragnar = gameObject.GetComponent<playerControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.GetButtonDown("RTrigger") && !m_isGrabbing)
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
        else if (player.GetButtonDown("RTrigger") && m_isGrabbing && !m_isThereAnAimer)
        {
            m_yVelocity = m_yVelocity * 0.1f;
            m_xVelocity = m_xVelocity * 0.1f;
            m_isThrowing = true;
        }
        else if (player.GetButtonDown("RTrigger") && m_isThereAnAimer)
        {
            m_cancelThrow = true;
            holdPoint.transform.rotation = m_tempHoldRotation;

            Destroy(createdAim[0]);
            createdAim.Clear();
            ragnar.m_throwMode = false;
            m_tempThrowDist = m_throwDistance;

            m_isThereAnAimer = false;

        }
        if ((player.GetAxisRawTimeInactive("RHorizontal") > 0.1f || player.GetAxisRawTimeInactive("RVertical") > 0.1f))
        {
            m_cancelThrow = false;
        }

        if (!m_cancelThrow)
        {
            if (player.GetAxis("RHorizontal") > 0)
            {
                transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.z);
            }
            if (player.GetAxis("RHorizontal") < 0)
            {
                transform.localScale = new Vector3(-0.6f, transform.localScale.y, transform.localScale.z);
            }
            if ((player.GetAxisRaw("RHorizontal") != 0 || player.GetAxisRaw("RVertical") != 0) && m_isGrabbing)
            {
                m_tempThrowDist += Time.deltaTime * m_rateOfThrowDistIncrease;
            }
            if ((player.GetAxisRaw("RHorizontal") != 0.0f || player.GetAxisRaw("RVertical") != 0.0f) && (m_isGrabbing && !m_isThereAnAimer))
            {
                ragnar.m_throwMode = true;
                m_createAimer = true;
            }
            else if ((player.GetAxisRaw("RHorizontal") == 0 && player.GetAxisRaw("RVertical") == 0) && m_isThereAnAimer)
            {
                m_isThrowing = true;
                holdPoint.transform.rotation = m_tempHoldRotation;

                Destroy(createdAim[0]);
                createdAim.Clear();
                ragnar.m_throwMode = false;
                m_tempThrowDist = m_throwDistance;

                m_isThereAnAimer = false;
            }
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

    void FixedUpdate()
    {
        if (ragnar.m_throwMode && m_isThereAnAimer)
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
        float horz = player.GetAxisRaw("RHorizontal");
        float vert = player.GetAxisRaw("RVertical");
        float tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        createdAim[0].transform.rotation = Quaternion.Euler(0, 0, tarAngle + 90);
        createdAim[0].transform.position = holdPoint.position + transform.forward * -0.5f;
    }

    void CreateAimer()
    {
        GameObject createdAimer;
        createdAimer = Instantiate(m_pointer, holdPoint.position + transform.forward * -0.5f, holdPoint.rotation);
        createdAim.Add(createdAimer);
    }

}