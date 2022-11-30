using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages interactions between player scripts, gameManagerScript, and other scripts
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    private PlayerHealth health;
    private PlayerMovement move;
    private PlayerJump jump;
    private PlayerRoll roll;

    public PlayerHealth Health { get => health; }
    public PlayerMovement Move { get => move; }
    public PlayerJump Jump { get => jump; }
    public PlayerRoll Roll { get => roll; }

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