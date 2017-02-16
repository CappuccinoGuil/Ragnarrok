using UnityEngine;
using System.Collections;

public class DwaneController : MonoBehaviour {

    [SerializeField] float m_moveSpeed = 3;
    [SerializeField] float m_spinSpeed = 30.0f;
    [SerializeField] float m_effectRadius = 2;
    [SerializeField] float m_pullForce = 2;
    [SerializeField] float m_pushForce = 5;
    private Rigidbody2D m_rb;

    void Start () {
        m_rb = GetComponent<Rigidbody2D>();
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
    }

    void MoveLeft()
    {
        m_rb.AddTorque(m_spinSpeed);
    }

    void MoveRight()
    {
        m_rb.AddTorque(-m_spinSpeed);
    }

    //void HandleInput()
    //{
    //    m_rb.AddForce(Vector2.left * (-m_moveSpeed * Input.GetAxis("Horizontal")));
    //}

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
