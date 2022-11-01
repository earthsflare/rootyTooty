using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : Player
{
    public static HealthDisplay instance { get; private set; }
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
