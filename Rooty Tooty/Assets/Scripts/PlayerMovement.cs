using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //TODO: Flip Character and create colliders for character and other objects.
    //TODO: For platforms and Floor give them the ground layer so the program knows when the character is on the ground and won't fall through

    public float regSpeed = 5f;
    public float characterSpeed;                //characterSpeed changes if the player chooses to sprint
    public float jumpHeight = 8f;
    public bool isJumping = false;
    public Transform ceilingCheck;              //make sure the character can't jump past a ceiling
    public Transform groundCheck;               //make sure the character doesn't fall through the ground
    public LayerMask groundObjects;             //layer to assign the platforms and ground to ground so that we can check when the player is landed.
    public float checkRadius;
    public int MAXJUMPS = 2;                       //Double Jumping or more

    private int jumpCounter;                    //Current amount of jumps
    private bool isGrounded;

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical

    private void start()
    {
        jumpCounter = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal");
        //        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)){
            characterSpeed = regSpeed * 2;
        }
        else
        {
            characterSpeed = regSpeed;
        }

        if (Input.GetButtonDown("Jump") && jumpCounter < MAXJUMPS)
        {
            isJumping = true;
        }


    }

    //Called many times per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * characterSpeed * Time.fixedDeltaTime);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);


        if (isGrounded)
        {
            jumpCounter = 0;
        }

        //Add jump force if the player used the jump key and perform a jump
        if (isJumping && jumpCounter < MAXJUMPS)
        {
            rb.AddForce(new Vector2(0f, jumpHeight));
            jumpCounter++;
        }
        isJumping = false;

    }
}
