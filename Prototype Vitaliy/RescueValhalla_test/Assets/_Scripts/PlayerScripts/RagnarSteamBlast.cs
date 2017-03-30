using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class RagnarSteamBlast : MonoBehaviour {

    private float m_launchForce;

    private Rigidbody2D m_rb;

    private playerControllerScript m_ragnar;

    private int playerId;
    private Player player; // The Rewired Player

    float CalculateLaunchForce(float Fvelocity, float time, float dist)
    {
        float acceleration = Fvelocity - 0 / time;
        float force = m_rb.mass * acceleration;
        return force = force / dist;
    }

    void Awake()
    {
        m_ragnar = gameObject.GetComponent<playerControllerScript>();
        playerId = m_ragnar.playerId;
        player = ReInput.players.GetPlayer(playerId); //Initializes the ReWired inputs  
    }

    void Start ()
    {
        m_rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
	    if(player.GetButtonDown("BButton"))
        {
            m_launchForce = CalculateLaunchForce(25f, 1f, 3f);
            m_rb.AddForce(transform.up * m_launchForce, ForceMode2D.Impulse);
        }	
	}
}
