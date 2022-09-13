using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    //Current action that the movement script is performing
    public Player.playerActions currentAction = Player.playerActions.idle;
    //List of actions that this script can interrupt
    private List<Player.playerActions> interruptableActions = new List<Player.playerActions>();
}
