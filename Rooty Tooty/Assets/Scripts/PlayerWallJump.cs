using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float wallJumpForcex = 1f;
    [SerializeField] private float wallJumpForcey = 1f;
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask WallLayer;

    public ParticleSystem dust;
    public ParticleSystem dustLeft;
    public ParticleSystem WallGust;
    public ParticleSystem WallGustLeft;

    private bool collideLeftWall = false;
    private bool collideRightWall = false;
    private int wallJumpDirection = 0;
    private bool collideWall { get => (collideLeftWall || collideRightWall); }
    private void StopCollideWall() { collideLeftWall = false; collideRightWall = false; }

    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool isWallJumping;

    [SerializeField] private AudioSource Jumpsfx;


    [Header("References")]
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;


    [HideInInspector] PlayerJump Jump;

    // Start is called before the first frame update
    void Start()
    {
        Jump = GetComponent<PlayerJump>();
    }


    // Update is called once per frame
    void Update()
    {
        collideRightWall = Physics2D.OverlapCircle(wallCheckRight.position, checkRadius, WallLayer);
        collideLeftWall = Physics2D.OverlapCircle(wallCheckLeft.position, checkRadius, WallLayer);

        if (collideWall && !Player.instance.jump.isGrounded /*&& Movement.movement.x != 0*/)
        {
            isSliding = true;
            Jump.jumpCounter = 0;
            Jump.animator.SetInteger("JumpCount", Jump.jumpCounter);
        }
        else
        {
            isSliding = false;
            dust.Stop();
            StopCollideWall();
        }
        /*        if (Jump.isGrounded)
                {
                    isSliding = false;
                    collideWall = false;
                }*/


        if (Input.GetButtonDown("Jump") && isSliding && !(collideLeftWall && collideRightWall))
        {
            if (collideRightWall)
            {
                wallJumpDirection = -1;

            }
            else
            {
                wallJumpDirection = 1;
            }

            isWallJumping = true;
            Invoke("SetWallJumping", wallJumpTime);
        }
        else if (!isWallJumping)
            wallJumpDirection = 0;

    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            if (collideRightWall)
            {
                dust.Play();
            }
            else
            {
                dustLeft.Play();
            }
            Player.instance.move.rb.velocity = new Vector2(Player.instance.move.rb.velocity.x, Mathf.Clamp(Player.instance.move.rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (isWallJumping && Jump.jumpCounter < 1)
        {
            StopCollideWall();
            isSliding = false;
            dust.Stop();
            dustLeft.Stop();

            if (wallJumpDirection < 0)
            {
                WallGust.Play();

            }
            else
            {
                WallGustLeft.Play();
            }

            Player.instance.move.canMove = false;
            Jumpsfx.Play();
            Player.instance.move.rb.velocity = new Vector2(wallJumpForcex * wallJumpDirection, wallJumpForcey);

            Jump.jumpCounter++;
            Jump.animator.SetInteger("JumpCount", Jump.jumpCounter);
        }
        /*if (isWallJumping && Jump.jumpCounter > 1)
        {
            Player.instance.move.canMove = true;
        }*/

    }

    void SetWallJumping()
    {
        isWallJumping = false;
        Player.instance.move.canMove = true;
    }


    //Time for sliding
    /*    private IEnumerator Slide()
        {
            yield return new WaitForSeconds(slideTime);
        }*/
}
