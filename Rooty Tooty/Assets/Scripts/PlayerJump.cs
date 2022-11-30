using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Properties")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private int MAXJUMPS = 1;                    //Double Jumping or more
    [SerializeField] private float TOTALJUMPTIME = 0.35f;
    [SerializeField] private float checkRadius = 0.05f; // How many units for groundCheck

    #region Jump Properties: Getter Setters
    public void SetMaxJumps(int i)
    {
        MAXJUMPS = i;
    }

    #endregion

    [Header("Debug: Read Only")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private int jumpCounter;                    //Current amount of jumps
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool jumpHold; //True when player is holding the jump button

    #region Jump Other: Getter Setters
    public int JumpCounter { get => jumpCounter; }
    public bool IsGrounded { get => isGrounded; }

    public void SetJumpCounter(int value) { jumpCounter = value; }
    public void AddJumpCounter(int value) { jumpCounter += value; }
    #endregion

    [Header("Object References")]
    [SerializeField] private Transform ceilingCheck;              //make sure the character can't jump past a ceiling
    [SerializeField] private Transform groundCheck;               //make sure the character doesn't fall through the ground
    [SerializeField] private LayerMask groundObjects;             //layer to assign the platforms and ground to ground so that we can check when the player is landed.
    [Space(10)]
    [SerializeField] private Animator animator;                   //Link the animator to this script so that it will change with the correct input
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem gust;                 //gust of wind
    [SerializeField] private ParticleSystem gust2;

    [HideInInspector] PlayerMovement Movement;
    [HideInInspector] PlayerWallJump WallJump;

    #region Jump Other: Getter Setters
    public Animator JumpAnimator { get => animator; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<PlayerMovement>();
        WallJump = GetComponent<PlayerWallJump>();

        jumpCounter = 0;
        animator.SetInteger("JumpCount", jumpCounter);
    }

    // Update is called once per frame
    void Update()
    {
        //Can't jump while in the middle of rolling animation
        if (!Movement.canMove)
        {
            return;
        }
        if (Movement.isRolling)
        {
            jumpHold = false;
            gust.Stop();
            gust2.Stop();
            isJumping = false;
            return;
        }
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpCounter < MAXJUMPS)
        {
            animator.SetBool("Jump", true);
            isJumping = true;
            jumpTimeCounter = TOTALJUMPTIME;
        }

        if (Input.GetButtonUp("Jump"))
        {
            gust.Stop();
            gust2.Stop();
            isJumping = false;
            jumpHold = false;
        }
    }

    void FixedUpdate()
    {
        if (!Movement.canMove)
        {
            return;
        }

        //Can't jump while in the middle of rolling animation
        if (Movement.isRolling)
        {
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            jumpCounter = 0;
            animator.SetInteger("JumpCount", jumpCounter);
        }
        animator.SetBool("Jump", !isGrounded);

        //Add jump velocity if the player used the jump key and perform a jump
        if (isJumping && (jumpCounter < MAXJUMPS) && !jumpHold)
        {
            //rb.velocity = Vector2.up * jumpHeight;
            //gust
            if (jumpCounter < 1)
            {
                gust.Play();
            }
            else
            {
                gust2.Play();
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

            jumpCounter++;
            animator.SetInteger("JumpCount", jumpCounter);
            jumpHold = true;
            
        }

        //For a long jump while holding down the button
        if (jumpHold)
        {
            if (jumpTimeCounter > 0)
            {
                //rb.velocity = Vector2.up * jumpHeight;
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);

                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                gust.Stop();
                gust2.Stop();
                isJumping = false;
                jumpHold = false;
            }
        }
    }

    public int getJump()
    {
        return MAXJUMPS;
    }

    public void enableDoubleJump()
    {
        MAXJUMPS = 2;
    }
}