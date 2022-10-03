using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    
    private Vector3 nextLevelPosition;
    //o generally better to use getters and setters instead of making variables public
    public Vector3 NextlevelPos { set => nextLevelPosition = value; }


    //Prefab that holds the player character
    [SerializeField] private GameObject playerPrefab;

    // ensures the character is not deleted upon loading a new scene + 
    // removes duplicating the character
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        /* o This function won't get called
        else if (instance != null)
        {
            gameObject.transform.position = nextLevelPosition;
        }
        */

        //o We want to change the position of the player character, not the gameManager script
        //gameObject.transform.position = nextLevelPosition;

        //We want to check if this script is the gameManager
        if (instance == this)
        {
            //o Probably going to need a player character to add don't destroy on load, but for now we can just find the player through FindObjectOfType
            if (playerPrefab == null)
                FindObjectOfType<PlayerMovement>().transform.position = nextLevelPosition;
        }
    }
}