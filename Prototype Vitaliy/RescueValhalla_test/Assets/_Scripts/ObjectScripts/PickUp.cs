using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

 //   [SerializeField] GameObject m_ragnarObject; 
    [SerializeField] bool m_steamPickUp;

    UIRagnar ragnarUI;

    void HandlePickUp()
    {
        if(m_steamPickUp)
        {
            ragnarUI.m_steamPickedUp = true;
            Destroy(gameObject);
        }
    }


    void Start ()
    {
	}
	
	void Update ()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Ragnar")
        {
            ragnarUI = col.GetComponent<UIRagnar>();

            HandlePickUp();
        }
    }


}
