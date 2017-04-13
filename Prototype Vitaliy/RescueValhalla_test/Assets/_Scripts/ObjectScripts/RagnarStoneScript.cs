using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RagnarStoneScript : MonoBehaviour {

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

	[HideInInspector] public bool m_horizontalMovement = true;

	private Rigidbody2D m_rb;
	private CircleCollider2D m_circleCollider;

	private Player player; // The Rewired Player

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody2D>();
		m_circleCollider = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake ()
	{
		player = ReInput.players.GetPlayer(0); //Initializes the ReWired inputs  
	}

	void FixedUpdate ()
	{

		if (player.GetButton("LBumper"))
		{
			PullEffect();
		}
		if (player.GetButton("RBumper"))
		{
			PushEffect();
		}
	
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

	void PullEffect()
	{

		Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
		foreach (Collider2D item in inRange)
		{
			Vector2 m_magDist = item.transform.position - transform.position;

			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Ragnar"))
			{
				m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
				item.attachedRigidbody.AddForce((m_magDist).normalized * -m_pullForce, ForceMode2D.Force); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
				m_rb.AddForce((m_magDist).normalized * m_pullForce , ForceMode2D.Force);
			}

//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
//			{
//				m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
//				item.attachedRigidbody.AddForce((m_magDist).normalized * -m_pullForce, ForceMode2D.Force); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
//				m_rb.AddForce((m_magDist).normalized * m_pullForce , ForceMode2D.Force);
//			}
//
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveVert"))
//			{
//				m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.x);
//				Vector2 dist = new Vector2(m_magDist.x, 0);
//				if (dist.x > 0)
//				{
//					m_rb.AddForce((dist).normalized * m_pullForce, ForceMode2D.Force);
//				}
//				if (dist.x < 0)
//				{
//					m_rb.AddForce((dist).normalized * -m_pullForce, ForceMode2D.Force);
//				}
//			}
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveHorz"))
//			{
//				m_pullForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.y);
//				Vector2 dist = new Vector2(0, m_magDist.y);
//				if (dist.y > 0)
//				{
//					m_rb.AddForce((dist).normalized * m_pullForce, ForceMode2D.Force);
//					m_angleOfForce = 180;
//				} else { m_angleOfForce = -180; }
//				if (dist.y < 0)
//				{
//					m_rb.AddForce((dist).normalized * -m_pullForce, ForceMode2D.Force);
//				}
//			}
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
//			{
//				m_pullForce = CalculateMagForce(m_finalRoundedPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
//				Vector2 dist = m_magDist.normalized;
//				if (dist.y > 0)
//				{
//					m_rb.AddForce((dist) * m_pullForce, ForceMode2D.Force);
//					m_angleOfForce = 180;
//				}
//				if (dist.y < 0)
//				{
//					m_rb.AddForce((dist) * m_pullForce, ForceMode2D.Force);
//					m_angleOfForce = -180;
//				}
//			}
		}
	}


	void PushEffect()
	{
		Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, m_effectRadius);
		foreach (Collider2D item in inRange)
		{
			Vector2 m_magDist = item.transform.position - transform.position;

			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Ragnar"))
			{
				m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.magnitude);
				item.attachedRigidbody.AddForce((m_magDist).normalized * m_pushForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a push force is applied
				m_rb.AddForce((m_magDist).normalized * -m_pushForce, ForceMode2D.Impulse);
			}

//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
//			{
//				m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.magnitude);
//				item.attachedRigidbody.AddForce((m_magDist).normalized * m_pushForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a push force is applied
//				m_rb.AddForce((m_magDist).normalized * -m_pushForce, ForceMode2D.Impulse);
//			}
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveVert"))
//			{
//				m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.x);
//				Vector2 dist = new Vector2(m_magDist.x, 0);
//				if (dist.x > 0)
//				{
//					m_rb.AddForce((dist).normalized * -m_pushForce, ForceMode2D.Impulse);
//				}
//				if (dist.x < 0)
//				{
//					m_rb.AddForce((dist).normalized * m_pushForce, ForceMode2D.Impulse);
//				}
//			}
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractiveHorz"))
//			{
//				m_pushForce = CalculateMagForce(m_finalPushVelocity, m_timeToSetPushVelocity, m_magDist.y);
//				Vector2 dist = new Vector2(0, m_magDist.y);
//				if (dist.y > 0)
//				{
//					m_rb.AddForce((dist).normalized * -m_pushForce, ForceMode2D.Impulse);
//				}
//				if (dist.y < 0)
//				{
//					m_rb.AddForce((dist).normalized * m_pushForce, ForceMode2D.Impulse);
//				}
//			}
//			if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
//			{
//				m_pushForce = CalculateMagForce(m_finalPullVelocity, m_timeToSetPullVelocity, m_magDist.magnitude);
//				Vector2 dist = m_magDist.normalized;
//				m_rb.AddForce((dist) * -m_pushForce, ForceMode2D.Impulse);
//			}
		}
	}


	void OnDrawGizmos()
	{

		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, m_effectRadius);

	}
}
