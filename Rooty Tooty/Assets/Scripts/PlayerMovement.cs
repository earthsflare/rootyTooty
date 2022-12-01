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
    [HideInInspector] public bool facingRight = true;            //Which direction the player sprite is facing

    public bool canRoll = false;
    /*[HideInInspector]*/ public bool canMove;
    [HideInInspector] public bool isRolling;

    public float knockBackPower;

    [HideInInspector] public GameObject Enemy;

    public Rigidbody2D rb;
    [HideInInspector] public Vector2 movement;                           //vectors store x and y horizontal and vertical

    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        rb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Enemy = GameObject.FindWithTag("Enemy");

        if (!canMove)
        {
            return;
        }
        if (isRolling && canRoll)
        {
            return;
        }
        if (isRolling)
        {
            return;
        }


        //gives -1 or 1 depending on input ex. left is -1 right is 1
        movement.x = Input.GetAxisRaw("Horizontal") * characterSpeed;

        //If user presses Shift the character will sprint this is for testing purposes
        if (Input.GetButton("Sprint"))
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
        if (!canMove)
        {
            return;
        }
        if (isRolling && canRoll)
        {
            return;
        }
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

    public void knockBack(Vector2 EnemyPos, Vector2 PlayerPos, Rigidbody2D rb, bool push, int knockbackTime)
    {
        if (push)
        {
            canMove = false;

            if (EnemyPos.x > PlayerPos.x)
            {
                rb.velocity = new Vector2(-knockBackPower, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(knockBackPower, rb.velocity.y);
            }

            // start coroutine
            StartCoroutine(knockBackTimer(knockbackTime));
        }
    }

    //Flips the character sprite if the movement direction is left or -1
    private void flipCharacter()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    private IEnumerator knockBackTimer(int time)
    {
        yield return new WaitForSeconds(time);

        canMove = true;
    }

    public bool getRoll()
    {
        return canRoll;
    }

    public void enableRoll()
    {
        canRoll = true;
    }
}
