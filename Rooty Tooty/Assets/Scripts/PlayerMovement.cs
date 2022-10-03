using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ctrl + shift + / will comment out

    public float regSpeed = 5f;
    public float characterSpeed = 0f;                //characterSpeed changes if the player chooses to sprint
    public Animator animator;                   //Link the animator to this script so that it will change with the correct input
    private bool facingRight = true;            //Which direction the player sprite is facing

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical


    // Update is called once per frame
    void Update()
    {
        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * characterSpeed;

        //User holds shift the character will sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterSpeed = regSpeed * 2;
        }
        else
        {
            characterSpeed = regSpeed;
        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //Called many times per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x, rb.velocity.y);


        if (movement.x > 0 && !facingRight)
        {
            flipCharacter();
        }
        else if (movement.x < 0 && facingRight)
        {
            flipCharacter();
        }

    }
    //Flips the character sprite if the movement direction is left or -1
    private void flipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
