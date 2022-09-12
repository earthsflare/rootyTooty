using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // ensures the character is not deleted upon loading a new scene + 
    // removes duplicating the character
        void Awake()
        {
            if (instance != null)
            {
            Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
}