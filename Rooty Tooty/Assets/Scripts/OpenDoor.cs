using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool playerDetected; // a boolean checking for if a player is detected in the range
    
    /*
    //o instead of this you can probably reference the position of the OpenDoor script. You don't need to make a child gameObect (tranform.position)
    public Transform doorPos; // empty game object beside the door, where the range starts

    //o instead of this you can reference the door width and height using transform.localScale.x & transform.localScale.y
    
    public float width; // width of rectangle to fit the door
    public float height; // height of rectangle to fit the door
    */

    //o Instead of using public variables you can use [SerializedField] to diplay the script in the inspector
    [SerializeField] private Vector2 nextDoorPos = Vector2.zero;

    [SerializeField] private LayerMask whatIsPlayer;// what to check for to change the bool

    [SerializeField]
    private string sceneName;

    SceneSwitch sceneSwitch;
    //GameManager gameManager;

    private void Start()
    {
        sceneSwitch = FindObjectOfType<SceneSwitch>();
    }

    private void Update()
    {
        // what playerdetected is
        // parameters tell what the origin is and how big/tall it is
        playerDetected = Physics2D.OverlapBox(transform.position, transform.lossyScale, 0, whatIsPlayer);
        if (playerDetected == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //o before entering the scene, set where you want the player to be at in the next scene
                levelManager.instance.SetNextLevelPos(nextDoorPos);
                levelManager.instance.LoadScene(sceneName);
            }
        }
    }

    // draws the box for the player to see
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.lossyScale.x, transform.lossyScale.y, 1)); 
    }
}
