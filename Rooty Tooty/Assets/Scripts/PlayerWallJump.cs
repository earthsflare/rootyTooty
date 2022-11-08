using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    public int jump;
    private bool collideWall = false;
    private bool canJump = false;
    private bool isSliding;
    private bool isWallJumping;
    public float slideTime;
    public float wallSlidingSpeed;
    public float wallJumpForce;

    public Transform wallCheck;
    public LayerMask WallLayer;

    [HideInInspector] PlayerMovement Movement;


    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void collidingWall()
    {

    }

    void isWallSliding()
    {

    }

    //Time for sliding
    private IEnumerator Slide()
    {

    }
}
