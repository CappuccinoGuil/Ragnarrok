using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour {
    [SerializeField] bool ragnarReady = false;
    [SerializeField] bool rokReady = false;
    
	void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ragnarReady = true;
        }

        if (other.CompareTag("Dwane"))
        {
            rokReady = true;
        }
    }
        
    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ragnarReady = false;
        }

        if (other.CompareTag("Dwane"))
        {
            rokReady = false;
        }
    }

    void Update()
    {
        if (ragnarReady && rokReady)
        {
            NextLevel();
        }
    
    }

    void NextLevel()
    {
        GetComponent<SceneTransition>().NextLevelButton("Level_02");
    }
}
