using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public loadlevel lLevel;
    public Vector3 nextLevelPosition;

    // ensures the character is not deleted upon loading a new scene + 
    // removes duplicating the character
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.transform.position = nextLevelPosition;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}