using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    
    public Vector3 nextLevelPosition;

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
        else if (instance != null)
        {
            gameObject.transform.position = nextLevelPosition;
        }
        gameObject.transform.position = nextLevelPosition;
    }
}