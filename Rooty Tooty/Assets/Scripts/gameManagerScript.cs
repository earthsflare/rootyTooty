using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gameManagerScript : MonoBehaviour
{
    public static gameManagerScript instance;

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
    }
}
