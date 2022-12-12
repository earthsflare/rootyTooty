using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SceneSwitch
{
    // creating it so that the player can exit from the door that was entered
    public Transform player;
    public float posX;
    public float posY;

    public string previous; // used for checking multiple doors 

    public override void Start()
    {
        // calls upon the start method in sceneswitch to not disregard it
        base.Start();

        if (prevScene == previous)
        {
            player.position = new Vector2(posX, posY);
        }
    }


}
