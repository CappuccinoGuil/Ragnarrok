using UnityEngine;
using System.Collections;

public class DwaneController : MonoBehaviour {


    [SerializeField] float m_finalVelocity = 10.0f; // In metres per second.
    [SerializeField] float m_timeToSetVelocity = 3.0f;
    [SerializeField] float m_angleOfForce = 90.0f;
    [SerializeField] float m_effectRadius = 2;
    [SerializeField] float m_pullForce = 2;
    [SerializeField] float m_pushForce = 5;

    public float m_checkVelocity;

    private float m_appliedForce;
    private float m_sumOfTorque;

    private Rigidbody2D m_rb;
    private CircleCollider2D m_circleCollider;
    


    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
        m_circleCollider = GetComponent<CircleCollider2D>();
        CalculateForce(m_finalVelocity, m_timeToSetVelocity);
        HandleTorque();
	}
	
	void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        if (Input.GetKey(KeyCode.S))
        {
            PullEffect();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            PushEffect();
        }
        m_rb.velocity = Vector2.ClampMagnitude(m_rb.velocity, m_finalVelocity);

        m_checkVelocity = m_rb.velocity.x;

    }

    void MoveLeft()
    {
        m_rb.AddTorque(-m_sumOfTorque);
    }

    void MoveRight()
    {
        m_rb.AddTorque(m_sumOfTorque);
    }

    float CalculateForce(float Fvelocity, float time)
    {
        float acceleration = 0 - Fvelocity / time;
        return m_appliedForce = m_rb.mass * acceleration;
    }


   void HandleTorque()
    {
        float inertia = 0.5f * m_rb.mass * Mathf.Pow(m_circleCollider.radius, 2.0f);
        float angle = Mathf.Sin(m_angleOfForce) / -0.5f;
        float angularAcceleration = (angle * m_appliedForce) / (m_rb.mass * m_circleCollider.radius);

        m_sumOfTorque = inertia * angularAcceleration;
    }

    void PullEffect()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
        foreach (Collider2D item in inRange)
        {
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
            {
                item.attachedRigidbody.AddForce((item.transform.position - transform.position).normalized * -m_pullForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
                m_rb.AddForce((item.transform.position - transform.position).normalized * m_pullForce, ForceMode2D.Force);
            }

            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {

                m_rb.AddForce((item.transform.position - transform.position).normalized * (m_pullForce * 3), ForceMode2D.Force);
            }
        }
    }


    void PushEffect()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
        foreach (Collider2D item in inRange)
        {
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
            {
                item.attachedRigidbody.AddForce((item.transform.position - transform.position).normalized * m_pushForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a push force is applied
                m_rb.AddForce((item.transform.position - transform.position).normalized * -m_pushForce, ForceMode2D.Impulse);
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {

                m_rb.AddForce((item.transform.position - transform.position).normalized * -m_pushForce, ForceMode2D.Impulse);
            }
        }
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, m_effectRadius);

    }
}
