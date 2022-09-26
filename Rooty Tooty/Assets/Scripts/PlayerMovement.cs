using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ctrl + shift + / will comment out

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
    private bool facingRight = true;            //Which direction the player sprite is facing

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical

    private void start()
    {
        jumpCounter = 0;
        animator.SetInteger("JumpCount", jumpCounter);
    }


    // Update is called once per frame
    void Update()
    {
        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * characterSpeed;

        //If user presses Shift the character will sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterSpeed = regSpeed * 2;
        }
        else
        {
            characterSpeed = regSpeed;
        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpCounter < MAXJUMPS)
        {
            animator.SetBool("Jump", true);
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
            jumpCounter = 0;
            animator.SetInteger("JumpCount", jumpCounter);
        }
        animator.SetBool("Jump", !isGrounded);

        //Add jump force if the player used the jump key and perform a jump
        if (isJumping && (jumpCounter < MAXJUMPS))
        {
            rb.velocity = Vector2.up * jumpHeight;
            jumpCounter++;
            animator.SetInteger("JumpCount", jumpCounter);
        }
        isJumping = false;

        if (movement.x > 0 && !facingRight)
        {
            flipCharacter();
        }
        else if (movement.x < 0 && facingRight)
        {
            flipCharacter();
        }

        // sends the player to the start position of each level
        //private void OnLevelWasLoaded(int level)
        //{
        //   transform.position = GameObject.FindWithTag("StartPos").transform.position;
        // }

    }
    //Flips the character sprite if the movement direction is left or -1
    private void flipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
