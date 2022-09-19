using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool playerDetected; // a boolean checking for if a player is detected in the range
    public Transform doorPos; // empty game object beside the door, where the range starts
    public float width; // width of rectangle to fit the door
    public float height; // height of rectangle to fit the door

    public LayerMask whatIsPlayer;// what to check for to change the bool

    [SerializeField]
    private string sceneName;

    SceneSwitch sceneSwitch;

    private void Start()
    {
        sceneSwitch = FindObjectOfType<SceneSwitch>();
    }

    private void Update()
    {
        // what playerdetected is
        // parameters tell what the origin is and how big/tall it is
        playerDetected = Physics2D.OverlapBox(doorPos.position, new Vector2(width, height), 0, whatIsPlayer);
        if (playerDetected == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sceneSwitch.SwitchScene(sceneName);
            }
        }
    }

    // draws the box for the player to see
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(doorPos.position, new Vector3(width, height, 1)); 
    }
}
