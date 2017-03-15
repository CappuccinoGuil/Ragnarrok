using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class RokController : MonoBehaviour {

    [SerializeField] float moveSpeed = 3;
    [SerializeField] float effectRadius = 4;
    [SerializeField] float pullForce = 2;
    [SerializeField] float pushForce = 5;
    private Rigidbody2D rb;
    
  
    //rewired
    public int playerId = 0;
    private Player player; // The Rewired Player


    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);//Initializes the ReWired inputs
    }


    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	

	void FixedUpdate () {
        if (player.GetAxis("LHorizontal") != 0)
        {
            HandleInput();
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

    void HandleInput() //Where input goes to be sorted
    {
        if (player.GetAxis("LHorizontal") != 0) 
        {
            rb.AddForce(Vector2.left * (-moveSpeed * player.GetAxis("LHorizontal")), ForceMode2D.Force); //rolls the rok in the direction of the left joystick
        }

       
      
    }

    void PullEffect()
    {
        
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, effectRadius);
      foreach (Collider2D item in inRange ) 
      {
      if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
        {
                 item.attachedRigidbody.AddForce((item.transform.position - transform.position).normalized * -pullForce, ForceMode2D.Force); // messy but if the detected collider has a rigidbody and is tagged as interactive a pull force is applied
                this.GetComponent<Rigidbody2D>().AddForce((item.transform.position - transform.position).normalized * pullForce, ForceMode2D.Force);
         }

            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {
                
                this.GetComponent<Rigidbody2D>().AddForce((item.transform.position - transform.position).normalized * (pullForce * 3), ForceMode2D.Force);
            }
        }
    }


    void PushEffect()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, effectRadius);
        foreach (Collider2D item in inRange)
        {
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("Interactive"))
            {
                item.attachedRigidbody.AddForce((item.transform.position - transform.position).normalized * pushForce, ForceMode2D.Impulse); // messy but if the detected collider has a rigidbody and is tagged as interactive a push force is applied
                this.GetComponent<Rigidbody2D>().AddForce((item.transform.position - transform.position).normalized * -pushForce, ForceMode2D.Impulse);
            }
            if (item.GetComponent<Rigidbody2D>() && item.CompareTag("FixedInteractive"))
            {

                this.GetComponent<Rigidbody2D>().AddForce((item.transform.position - transform.position).normalized * -pushForce, ForceMode2D.Impulse);
            }
        }
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, effectRadius);

    }
}
