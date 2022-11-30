using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages interactions between player scripts, gameManagerScript, and other scripts
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    private PlayerAim aim;
    private PlayerHealth health;
    private PlayerJump jump;
    private PlayerMovement move;
    private PlayerRoll roll;
    private PlayerWallJump wallJump;

    public PlayerAim Aim { get => aim; }
    public PlayerHealth Health { get => health; }
    public PlayerJump Jump { get => jump; }
    public PlayerMovement Move { get => move; }
    public PlayerRoll Roll { get => roll; }
    public PlayerWallJump WallJump { get => wallJump; }

    public void ToggleRoll(bool enabled) { roll.enabled = enabled; }
    public void ToggleWallJump(bool enabled) { wallJump.enabled = enabled; }

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