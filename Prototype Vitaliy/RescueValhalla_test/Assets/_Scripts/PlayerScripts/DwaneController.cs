﻿using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class DwaneController: MonoBehaviour {

    [SerializeField] float m_finalVelocity = 9.0f; // In metres per second.
    [SerializeField] float m_finalPullVelocity = 23.0f;
    [SerializeField] float m_finalRoundedPullVelocity = 55.0f;
    [SerializeField] float m_finalPushVelocity = 10.0f;
    [SerializeField] float m_timeToSetVelocity = 3.0f;
    [SerializeField] float m_timeToSetPullVelocity = 1.0f;
    [SerializeField] float m_timeToSetPushVelocity = 1.0f;
    [SerializeField] float m_effectRadius = 2;

    private float m_appliedForce;
    private float m_sumOfTorque;
    private float m_pullForce;
    private float m_pushForce;
    private float m_angleOfForce = -180.0f;
    private float m_maxPullPushForce;
    private float m_minPullPushForce;

    [HideInInspector] public bool m_horizontalMovement = true;

    private Rigidbody2D m_rb;
    private CircleCollider2D m_circleCollider;
    private Animator m_myAnim;

    //rewired
    [SerializeField] int playerId = 1;
    private Player player; // The Rewired Player

    void Awake ()
    {
        player = ReInput.players.GetPlayer(playerId); //Initializes the ReWired inputs  
    }

    void Start ()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_myAnim = GetComponent<Animator>();
        m_circleCollider = GetComponent<CircleCollider2D>();
        CalculateForce(m_finalVelocity, m_timeToSetVelocity);

        m_maxPullPushForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, 0.8f);
        m_minPullPushForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, -0.8f);
    }

    void Update()
    {
        if(player.GetButtonUp("AButton"))
        {
            m_angleOfForce = -180;
        }
    }

    void FixedUpdate ()
    {
        if (m_horizontalMovement)
        {
            if (player.GetAxis("LHorizontal") != 0)
            {
                HandleTorque();
                m_rb.AddTorque(-m_sumOfTorque * player.GetAxis("LHorizontal"));
            }
        }
        if (!m_horizontalMovement)
        {
            if (player.GetAxis("LVertical") != 0)
            {
                HandleTorque();
                m_rb.AddTorque(-m_sumOfTorque * player.GetAxis("LVertical"));
            }
        }
        if (player.GetButton("AButton"))
        {
            PullEffect();
            m_myAnim.SetBool("Attract", true);
        }

        if (player.GetButtonDown("BButton"))
        {
            PushEffect();
            m_myAnim.SetTrigger("Repel");
        }

        m_rb.velocity = new Vector2(Mathf.Clamp(m_rb.velocity.x, -m_finalVelocity, m_finalVelocity), Mathf.Clamp(m_rb.velocity.y, -m_finalVelocity, m_finalVelocity));

        

    }

    float CalculateMagForce(float Fvelocity, float time, float dist)
    {
        float acceleration = Fvelocity - 0 / time;
        float force = m_rb.mass * acceleration;
        return force = force / dist;
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
            
            m_pullForce = Mathf.Clamp(m_pullForce, m_minPullPushForce, m_maxPullPushForce);

            //Collider2D closestCollider = item;
            //if (Vector2.Distance(item.transform.position, transform.position) < Vector2.Distance(closestCollider.transform.position,transform.position))
            //{
            //    closestCollider = item;
            //}

                Vector2 m_magDist = item.transform.position - transform.position;



                if (item.GetComponent<Rigidbody2D>() && (item.CompareTag("Interactive") ||item.CompareTag("InteractiveBox") || item.CompareTag("Ragnar")))
            {
                m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
                item.attachedRigidbody.AddForce((m_magDist).normalized * -m_pullForce, ForceMode2D.Force); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
                m_rb.AddForce((m_magDist).normalized * m_pullForce , ForceMode2D.Force);
            }

            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveVert"))
            {
                m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.x);
                
                Vector2 dist = new Vector2(m_magDist.x, 0);
                if (dist.x > 0)
                {
                    m_rb.AddForce((dist).normalized * m_pullForce, ForceMode2D.Force);
                }
                if (dist.x < 0)
                {
                    m_rb.AddForce((dist).normalized * -m_pullForce, ForceMode2D.Force);
                }
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveHorz"))
            {
                m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.y);
                Vector2 dist = new Vector2(0, m_magDist.y);
                if (dist.y > 0)
                {
                    m_rb.AddForce((dist).normalized * m_pullForce, ForceMode2D.Force);
                    m_angleOfForce = 180;
                } else { m_angleOfForce = -180; }
                if (dist.y < 0)
                {
                    m_rb.AddForce((dist).normalized * -m_pullForce, ForceMode2D.Force);
                }
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {
                m_pullForce = CalculateMagForce(m_finalRoundedPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
                Vector2 dist = m_magDist.normalized;
                if (dist.y > 0)
                {
                    m_rb.AddForce((dist) * m_pullForce, ForceMode2D.Force);
                    m_angleOfForce = 180;
                }
                if (dist.y < 0)
                {
                    m_rb.AddForce((dist) * m_pullForce, ForceMode2D.Force);
                    m_angleOfForce = -180;
                }
            }
        }
    }


    void PushEffect()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
        foreach (Collider2D item in inRange)
        {
            m_pushForce = Mathf.Clamp(m_pullForce, m_minPullPushForce, m_maxPullPushForce);

            Vector2 m_magDist = item.transform.position - transform.position;

            if (item.GetComponent<Rigidbody2D>() && (item.CompareTag("Interactive") || item.CompareTag("InteractiveBox")|| item.CompareTag("Ragnar")))
            {
                m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.magnitude);
                item.attachedRigidbody.AddForce((m_magDist).normalized * m_pushForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a push force is applied
                m_rb.AddForce((m_magDist).normalized * -m_pushForce, ForceMode2D.Impulse);
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveVert"))
            {
                m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.x);
                Vector2 dist = new Vector2(m_magDist.x, 0);
                if (dist.x > 0)
                {
                    m_rb.AddForce((dist).normalized * -m_pushForce, ForceMode2D.Impulse);
                }
                if (dist.x < 0)
                {
                    m_rb.AddForce((dist).normalized * m_pushForce, ForceMode2D.Impulse);
                }
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveHorz"))
            {
                m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.y);
                Vector2 dist = new Vector2(0, m_magDist.y);
                if (dist.y > 0)
                {
                    m_rb.AddForce((dist).normalized * -m_pushForce, ForceMode2D.Impulse);
                }
                if (dist.y < 0)
                {
                    m_rb.AddForce((dist).normalized * m_pushForce, ForceMode2D.Impulse);
                }
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {
                m_pushForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
                Vector2 dist = m_magDist.normalized;
                m_rb.AddForce((dist) * -m_pushForce, ForceMode2D.Impulse);
            }
        }
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, m_effectRadius);

    }
}
