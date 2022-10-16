using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ctrl + shift + / will comment out

    public float regSpeed = 5f;
    private float characterSpeed;
    //public float characterSpeed = 0f;                //characterSpeed changes if the player chooses to sprint
    public Animator animator;                   //Link the animator to this script so that it will change with the correct input
    private bool facingRight = true;            //Which direction the player sprite is facing

    //Player Roll Variables
    private bool canRoll = true;
    [HideInInspector] public bool isRolling;
    public float rollingSpd = 2f;
    public float rollingTime = 1f;
    public float rollingCooldown = 1f;

    int BushLayer;
    int PlayerLayer;

    public Rigidbody2D rb;
    Vector2 movement;                           //vectors store x and y horizontal and vertical

    void Start()
    {
        BushLayer = LayerMask.NameToLayer("RollBlock");
        PlayerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isRolling)
        {
            return;
        }


        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * characterSpeed;

        //If user presses Shift the character will sprint this is for testing purposes
        if (Input.GetKey(KeyCode.LeftShift))
        {
            characterSpeed = regSpeed * 2;
        }
        else
        {
            characterSpeed = regSpeed;
        }

        //Player Roll
        if (Input.GetKey(KeyCode.X) && canRoll)
        {
            StartCoroutine(Roll());
        }

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //Called many times per frame
    void FixedUpdate()
    {
        if (isRolling)
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

    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;
        animator.SetBool("isRolling", isRolling);

        //We don't want gravity to affect character while roll/dash in air
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
        Physics2D.IgnoreLayerCollision(BushLayer, PlayerLayer, true);
        rb.velocity = new Vector2(movement.x * rollingSpd, 0f);

        yield return new WaitForSeconds(rollingTime);
        rb.gravityScale = currentGravity;
        isRolling = false;
        animator.SetBool("isRolling", isRolling);
        Physics2D.IgnoreLayerCollision(BushLayer, PlayerLayer, false);

        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
    }


    // sends the player to the start position of each level
    //private void OnLevelWasLoaded(int level)
    //{
    //   transform.position = GameObject.FindWithTag("StartPos").transform.position;
    // }
}
