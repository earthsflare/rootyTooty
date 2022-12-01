using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages interactions between player scripts, gameManagerScript, and other scripts
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    [SerializeField] private PlayerAim aim;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerJump jump;
    [SerializeField] private PlayerMovement move;
    [SerializeField] private PlayerRoll roll;
    [SerializeField] private PlayerWallJump wallJump;

    public PlayerAim Aim { get => aim; }
    public PlayerHealth Health { get => health; }
    public PlayerJump Jump { get => jump; }
    public PlayerMovement Move { get => move; }
    public PlayerRoll Roll { get => roll; }
    public PlayerWallJump WallJump { get => wallJump; }

    public static void DeactivatePlayer() { instance.gameObject.SetActive(false); }
    public static void ActivatePlayer() { instance.gameObject.SetActive(true); }

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

        if(aim == null)
            aim = GetComponent<PlayerAim>();
        if (health == null)
            health = GetComponent<PlayerHealth>();
        if (jump == null)
            jump = GetComponent<PlayerJump>();
        if (move == null)
            move = GetComponent<PlayerMovement>();
        if (roll == null)
            roll = GetComponent<PlayerRoll>();
        if (wallJump == null)
            wallJump = GetComponent<PlayerWallJump>();
    }
}