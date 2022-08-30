using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;       //access to the player
/*    public float offset;
    public float offsetSmoothing;
    private Vector3 playerPos;      //player position*/

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

    }
}
