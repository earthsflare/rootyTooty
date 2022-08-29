using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ctrl + shift + / will comment out

    //TODO: Flip Character and create colliders for character and other objects.
    //TODO: For platforms and Floor give them the ground layer so the program knows when the character is on the ground and won't fall through

    public float regSpeed = 5f;
    public float characterSpeed = 0f;                //characterSpeed changes if the player chooses to sprint
    public float jumpHeight = 8f;
    public bool isJumping = false;
    public Transform ceilingCheck;              //make sure the character can't jump past a ceiling
    public Transform groundCheck;               //make sure the character doesn't fall through the ground
    public LayerMask groundObjects;             //layer to assign the platforms and ground to ground so that we can check when the player is landed.
    public float checkRadius;
    public int MAXJUMPS = 2;                    //Double Jumping or more
    public Animator animator;                   //Link the animator to this script so that it will change with the correct input

    public int jumpCounter;                    //Current amount of jumps
    private bool isGrounded;

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical

    private void start()
    {
        jumpCounter = MAXJUMPS;
    }


    // Update is called once per frame
    void Update()
    {
        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * characterSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterSpeed = regSpeed * 2;
        }
        else
        {
            characterSpeed = regSpeed;
        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));

        if (Input.GetButtonDown("Jump") && jumpCounter > 0)
        {
            isJumping = true;
        }


    }

    //Called many times per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        if (isGrounded)
        {
            jumpCounter = MAXJUMPS;
        }

        //Add jump force if the player used the jump key and perform a jump
        if (isJumping && (jumpCounter > 0))
        {
            Debug.Log("Add Force!");
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            jumpCounter--;
        }
        isJumping = false;

    }
}
