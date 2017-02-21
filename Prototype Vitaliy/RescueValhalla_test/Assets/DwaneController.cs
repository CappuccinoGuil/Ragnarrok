using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class DwaneController: MonoBehaviour {

    [SerializeField] float m_moveSpeed = 3;
    [SerializeField] float m_spinSpeed = 30.0f;
    [SerializeField] float m_effectRadius = 2;
    [SerializeField] float m_pullForce = 2;
    [SerializeField] float m_pushForce = 5;
    [SerializeField] float m_reactionaryForce = 5;
    private Rigidbody2D m_rb;


    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake ()
    {
        player = ReInput.players.GetPlayer(playerId); //Initializes the ReWired inputs  
    }

    void Start ()
    {
        player = ReInput.players.GetPlayer(playerId); //Initializes the ReWired inputs
        m_rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
        if (player.GetAxis("LHorizontal") != 0)
        {
            MoveRock();
        }
        if (player.GetButton("AButton"))
        {
            PullEffect();
        }
        if (player.GetButtonDown("BButton"))
        {
            PushEffect();
        }
    }

    void MoveRock()
    {
        m_rb.AddTorque( -m_spinSpeed * player.GetAxis("LHorizontal"));
    }
    void PullEffect()
    {

        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
        foreach (Collider2D item in inRange)
        {
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
            {
                item.attachedRigidbody.AddForce((item.transform.position - transform.position).normalized * -m_pullForce, ForceMode2D.Force); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
                m_rb.AddForce((item.transform.position - transform.position).normalized * m_pullForce , ForceMode2D.Force);
            }

            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {

                m_rb.AddForce((item.transform.position - transform.position).normalized * m_pullForce * m_reactionaryForce, ForceMode2D.Force);
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

                m_rb.AddForce((item.transform.position - transform.position).normalized * -m_pushForce * m_reactionaryForce, ForceMode2D.Impulse);
            }
        }
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, m_effectRadius);

    }
}
