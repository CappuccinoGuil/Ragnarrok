using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class RagnarSteamBlast : MonoBehaviour {

    [HideInInspector] public List<GameObject> createdAim;

    [SerializeField] Transform m_aimPoint;
    [SerializeField] GameObject m_pointer;

    [HideInInspector] public float m_launchDistance = 1f;

    [HideInInspector] public bool m_launch = false;

    private float m_launchForce;
    private float m_finalVelocity;
    private float m_acceleration;
    private float m_tarAngle;

    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_cancelBlast = false;

    private Vector3 m_launchDirection;

    private Rigidbody2D m_rb;

    private RagnarThrow m_throwScript;
    private playerControllerScript m_ragnar;

    private int playerId;
    private Player m_player; // The Rewired Player

    float CalculateFinalVelocity(float dist, float time, float initVelocity)
    {
        return (dist / time) - initVelocity / 2;
    }
    float CalculateAcceleration(float finalVelocity, float initVelocity, float time)
    {
        return (finalVelocity - initVelocity) / time;
    }
    float CalculateLaunchForce(float mass ,float acceleration)
    {
        return mass * acceleration;
    }

    void ApplyForce(float angle)
    {
        Collider2D[] inRange = Physics2D.OverlapBoxAll(m_aimPoint.position, new Vector2(3, 3), angle);
        foreach (Collider2D item in inRange)
        {
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("WoodenObject"))
            {
                item.attachedRigidbody.AddForce(m_launchDirection * -m_launchForce, ForceMode2D.Impulse);
            }
            if (m_ragnar.GroundCheck())
            {
                m_rb.AddForce(m_launchDirection * m_launchForce, ForceMode2D.Impulse);
            }
            if (!m_ragnar.GroundCheck())
            {
                m_rb.AddForce(m_launchDirection * (m_launchForce * 1.5f), ForceMode2D.Impulse);
            }

        }
    }

    void RotateAimer()
    {
        float horz = m_player.GetAxisRaw("RHorizontal");
        float vert = m_player.GetAxisRaw("RVertical");
        m_tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        createdAim[0].transform.rotation = Quaternion.Euler(0, 0, m_tarAngle + 90);
        createdAim[0].transform.position = m_aimPoint.position + transform.forward * -0.5f;
    }

    void CreateAimer()
    {
        GameObject createdAimer;
        createdAimer = Instantiate(m_pointer, m_aimPoint.position + transform.forward * -0.5f, m_aimPoint.rotation);
        createdAim.Add(createdAimer);
    }

    void Awake()
    {
        m_ragnar = gameObject.GetComponent<playerControllerScript>();
        playerId = m_ragnar.playerId;
        m_player = ReInput.players.GetPlayer(playerId); //Initializes the ReWired inputs  
    }

    void Start ()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_throwScript = GetComponent<RagnarThrow>();
	}
	
	void Update ()
    {
        if (!m_cancelBlast)
        {
            //Start aiming steam blast.
            if ((m_player.GetAxisRaw("RHorizontal") != 0.0f || m_player.GetAxisRaw("RVertical") != 0.0f) && (!m_throwScript.m_isGrabbing && !m_isThereAnAimer))
            {
                m_finalVelocity = CalculateFinalVelocity(m_launchDistance, 1, 0);
                m_acceleration = CalculateAcceleration(m_finalVelocity, 0, 1);
                m_launchForce = CalculateLaunchForce(m_rb.mass, m_acceleration);
                m_ragnar.m_blastMode = true;
                m_createAimer = true;
            }
            //Shoot steam blast.
            else if ((m_player.GetAxisRaw("RHorizontal") == 0 && m_player.GetAxisRaw("RVertical") == 0) && m_isThereAnAimer)
            {
                Destroy(createdAim[0]);
                createdAim.Clear();

                m_launch = true;
                m_ragnar.m_blastMode = false;
                m_isThereAnAimer = false;
            }
        }
        //Cancel Steam blast.
        if(m_player.GetButtonDown("RTrigger") && m_isThereAnAimer)
        {
            m_cancelBlast = true;
            Destroy(createdAim[0]);
            createdAim.Clear();
            m_ragnar.m_blastMode = false;
            m_isThereAnAimer = false;
        }
        //Set cancel throw to false when joy stick has been inactive for a moment.
        if ((m_player.GetAxisRawTimeInactive("RHorizontal") > 0.1f || m_player.GetAxisRawTimeInactive("RVertical") > 0.1f))
        {
            m_cancelBlast = false;
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
        if (m_isThereAnAimer)
        {
            RotateAimer();
            m_launchDirection = createdAim[0].transform.rotation * Vector3.down;
        }
        if(m_launch)
        {
            ApplyForce(m_tarAngle);
            m_launch = false;
        }
    }
}
