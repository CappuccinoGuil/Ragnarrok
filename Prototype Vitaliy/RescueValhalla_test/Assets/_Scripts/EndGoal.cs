using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour {
    [Header("Player Refrence")]
    [SerializeField] bool ragnarReady = false;
    [SerializeField] bool rokReady = false;

    [Header("Level Transitions")]
    [SerializeField] string nextLevel = "Level_01";
    [SerializeField] float loadDelay= 5f;

    [Header("Object Interactions")]
    [SerializeField] GameObject movingPlatform;
    [SerializeField] GameObject[] walls;
    

    void OnTriggerStay2D(Collider2D other)
    {       
        if (other.CompareTag("Ragnar"))
        {
            ragnarReady = true;
        }

        if (other.CompareTag("Dwane"))
        {
            rokReady = true;
        }
    }
        
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Ragnar"))
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
        if (movingPlatform != null)
        {
            foreach (GameObject wall in walls)
            {
                wall.SetActive(true);
            }
            movingPlatform.GetComponent<MovingPlatform>().enabled = true;
        }
        StartCoroutine(LoadLevel(nextLevel));
    }

    IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitForSeconds(loadDelay);
        GetComponent<SceneTransition>().NextLevelButton(nextLevel);
    }
}
 