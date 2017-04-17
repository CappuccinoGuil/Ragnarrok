using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class RagnarSteamBlast : MonoBehaviour {

    [HideInInspector] public List<GameObject> m_createdAim;
    [HideInInspector] public List<GameObject> m_createdEffect;
    [HideInInspector] public int m_switchEffectSteamBlast;

    [SerializeField] Transform m_aimPoint;

    [SerializeField] GameObject m_pointer;

    [Header("Steam Blast Effects")]
    [SerializeField] GameObject m_EffectLargeSteamBlast;
    [SerializeField] GameObject m_EffectMediumSteamBlast;
    [SerializeField] GameObject m_EffectSmallSteamBlast;

    [SerializeField] float m_SteamSpurtDistance = 3f; 

    [HideInInspector] public float m_launchDistance = 1f;

    [HideInInspector] public bool m_launch = false;
    [HideInInspector] public bool m_canSpurt = true;

    private float m_launchForce;
    private float m_finalVelocity;
    private float m_acceleration;
    private float m_tarAngle;

    private bool m_createAimer = false;
    private bool m_isThereAnAimer = false;
    private bool m_steamBlast = false;
    private bool m_cancelBlast = false;

    private bool m_steamSpurt = false;

    private Vector3 m_launchDirection;

    private Rigidbody2D m_rb;

    private RagnarThrow m_throwScript;
    private playerControllerScript m_ragnar;
    private UIRagnar m_ragUI;

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
    void HandleSteamSpurt()
    {
        if (m_ragnar.m_facingRight)
        {
            float finalVelocity = CalculateFinalVelocity(m_SteamSpurtDistance, 0.8f, m_rb.velocity.x);
            float acceleration = CalculateAcceleration(finalVelocity, m_rb.velocity.x, 0.8f);
            float spurtForce = CalculateLaunchForce(m_rb.mass, acceleration);

            float finalVelocityUp = CalculateFinalVelocity(1f, 0.8f, m_rb.velocity.y);
            float accelerationUp = CalculateAcceleration(finalVelocityUp, m_rb.velocity.y, 0.8f);
            float spurtForceUp = CalculateLaunchForce(m_rb.mass, accelerationUp);

            m_rb.AddForce(transform.up * spurtForceUp, ForceMode2D.Impulse);
            m_rb.AddForce(transform.right * spurtForce, ForceMode2D.Impulse);
            m_steamSpurt = false;
        }
        if (!m_ragnar.m_facingRight)
        {
            float finalVelocity = CalculateFinalVelocity(m_SteamSpurtDistance, 0.8f, -m_rb.velocity.x);
            float acceleration = CalculateAcceleration(finalVelocity, -m_rb.velocity.x, 0.8f);
            float spurtForce = CalculateLaunchForce(m_rb.mass, acceleration);

            float finalVelocityUp = CalculateFinalVelocity(1f, 0.8f, m_rb.velocity.y);
            float accelerationUp = CalculateAcceleration(finalVelocityUp, m_rb.velocity.y, 0.8f);
            float spurtForceUp = CalculateLaunchForce(m_rb.mass, accelerationUp);

            m_rb.AddForce(transform.up * spurtForceUp, ForceMode2D.Impulse);
            m_rb.AddForce(transform.right * -spurtForce, ForceMode2D.Impulse);
            m_steamSpurt = false;
        }
    }

    void HandleSteamBlast()
    {
            m_finalVelocity = CalculateFinalVelocity(m_launchDistance, 0.8f, m_rb.velocity.y);
            m_acceleration = CalculateAcceleration(m_finalVelocity, m_rb.velocity.y, 0.8f);
            m_launchForce = CalculateLaunchForce(m_rb.mass, m_acceleration);
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

        }
        m_rb.AddForce(m_launchDirection * m_launchForce, ForceMode2D.Impulse);

    }

    void RotateAimer()
    {
        float horz = m_player.GetAxisRaw("RHorizontal");
        float vert = m_player.GetAxisRaw("RVertical");
        m_tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg;

        m_createdAim[0].transform.rotation = Quaternion.Euler(0, 0, m_tarAngle + 90);
        m_createdAim[0].transform.position = m_aimPoint.position + transform.forward;


    }

    void CreateAimer()
    {
        GameObject createdAimer;
        createdAimer = Instantiate(m_pointer, m_aimPoint.position + transform.forward * -0.5f, m_aimPoint.rotation);
        m_createdAim.Add(createdAimer);
    }

    void SteamBlastEffect()
    {
        Quaternion EffectSteamBlastRot = Quaternion.Euler(0, 0, m_tarAngle);
        Vector3 EffectSteamBlastPos = m_aimPoint.position + transform.forward;

        GameObject createdEffect;
        switch (m_switchEffectSteamBlast)
        {

            case 1:
                createdEffect = Instantiate(m_EffectSmallSteamBlast, EffectSteamBlastPos, EffectSteamBlastRot);
                m_createdEffect.Add(createdEffect);
                StartCoroutine(WaitToStopEffect());
                break;
            case 2:
                createdEffect = Instantiate(m_EffectMediumSteamBlast, EffectSteamBlastPos, EffectSteamBlastRot);
                m_createdEffect.Add(createdEffect);
                StartCoroutine(WaitToStopEffect());
                break;
            case 3:
                createdEffect = Instantiate(m_EffectLargeSteamBlast, EffectSteamBlastPos, EffectSteamBlastRot);
                m_createdEffect.Add(createdEffect);
                StartCoroutine(WaitToStopEffect());
                break;
            default:
                Debug.Log("No Blast Effect");
                break;
        }  
    }

    void SteamSpurtEffect()
    {
        Quaternion EffectSteamSpurtRot = Quaternion.Euler(0, 0, 180);

        GameObject createdEffect;
        if(m_ragnar.m_facingRight)
        {
            createdEffect = Instantiate(m_EffectSmallSteamBlast, transform.position, transform.rotation);
            m_createdEffect.Add(createdEffect);
        }
        else
        {
            createdEffect = Instantiate(m_EffectSmallSteamBlast, transform.position, EffectSteamSpurtRot);
            m_createdEffect.Add(createdEffect);
        }
 
        StartCoroutine(WaitToStopEffect());
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
        m_ragUI = GetComponent<UIRagnar>();
	}
	
	void Update ()
    {

        if (m_player.GetButtonDown("XButton") && !m_throwScript.m_isGrabbing)
        {
            m_steamSpurt = true;
            m_ragUI.m_steamSpurtLaunched = true;
        }


        if (!m_cancelBlast)
        {
            //Start aiming steam blast.
            if ((m_player.GetAxisRaw("RHorizontal") != 0.0f || m_player.GetAxisRaw("RVertical") != 0.0f) && (!m_throwScript.m_isGrabbing && !m_isThereAnAimer))
            {
                m_steamBlast = true;
                m_ragnar.m_blastMode = true;
                m_createAimer = true;
            }
            //Shoot steam blast.
            else if ((m_player.GetAxisRaw("RHorizontal") == 0 && m_player.GetAxisRaw("RVertical") == 0) && m_isThereAnAimer)
            {
                Destroy(m_createdAim[0]);
                m_createdAim.Clear();

                m_launch = true;
                m_ragnar.m_blastMode = false;
                m_isThereAnAimer = false;
            }
        }
        //Cancel Steam blast.
        if(m_player.GetButtonDown("RTrigger") && m_isThereAnAimer)
        {
            m_cancelBlast = true;
            Destroy(m_createdAim[0]);
            m_createdAim.Clear();
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
            m_launchDirection = Quaternion.AngleAxis(m_tarAngle, Vector3.forward) * Vector3.right;
        }
        if(m_steamBlast)
        {
            HandleSteamBlast();
        }
        if(m_launch)
        {
            SteamBlastEffect();
            ApplyForce(m_tarAngle);
            m_ragUI.m_steamBlastLaunched = true;
            m_steamBlast = false;
            m_launch = false;
        }
        if (m_steamSpurt && m_canSpurt)
        {
            SteamSpurtEffect();
            HandleSteamSpurt();
        }
    }

    IEnumerator WaitToStopEffect()
    {
        yield return new WaitForSecondsRealtime(0.35f);
        if(m_createdEffect.Count > 0)
        {
            foreach(GameObject go in m_createdEffect)
            {
                go.GetComponent<ParticleSystem>().Stop();
            }
        }
        yield return new WaitForSecondsRealtime(0.35f);
        if (m_createdEffect.Count > 0)
        {
            foreach (GameObject go in m_createdEffect)
            {
                Destroy(go);
            }
        }
        m_createdEffect.Clear();
    }
}
