using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ctrl + shift + / will comment out

    public float regSpeed = 5f;
    //public float characterSpeed = 0f;                //characterSpeed changes if the player chooses to sprint
    public Animator animator;                   //Link the animator to this script so that it will change with the correct input
    private bool facingRight = true;            //Which direction the player sprite is facing

    //Player Dash Variables
    private bool canDash = true;
    [HideInInspector] public bool isDashing;
    public float dashingSpd = 2f;
    public float dashingTime = 1f;
    public float dashingCooldown = 1f;

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical


    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }


        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * regSpeed;

        //Player Dash
        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //Called many times per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", isDashing);

        //We don't want gravity to affect character while dashing in air
        float currentGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        if (movement.x == 0)
        {
            if (!facingRight)
            {
                movement.x = -1 * regSpeed;
            }
            else
            {
                movement.x = 1 * regSpeed;
            }
        }
        rb.velocity = new Vector2(movement.x * dashingSpd, 0f);

        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = currentGravity;
        isDashing = false;
        animator.SetBool("isDashing", isDashing);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
