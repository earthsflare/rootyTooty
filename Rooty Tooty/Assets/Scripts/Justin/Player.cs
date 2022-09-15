using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementScript = null;
    public enum playerActions
    {
        walking = 0,
        idle = 1,
        airTime = 2,
        groundSliding = 3,
        wallSliding = 4,
        wallJumping = 5,
        grappleHook = 6,
        parrying = 7,
        dropPound = 8,
        groundPound = 9
    }

    //Checks if player is moving through the PlayerMovement script (in the air or moving left or right
    private bool hasAction = false;
    public bool HasAction { set => hasAction = value; }
    private playerActions currentAction = playerActions.idle;

    public playerActions CurrentAction { get => currentAction; set => currentAction = value; }


    public void Update()
    {
        if (!hasAction)
        {
            currentAction = movementScript.currentAction;
        }
    }

    public void FinishAction()
    {
        hasAction = false;
    }
}
