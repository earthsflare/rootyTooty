using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    private bool collideWall;
    private bool isSliding;
    public float wallSlidingSpeed;

    [HideInInspector] public bool isWallJumping;
    public float wallJumpForcex;
    public float wallJumpForcey;
    public float wallJumpTime;

    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public LayerMask WallLayer;

    [HideInInspector] PlayerMovement Movement;
    [HideInInspector] PlayerJump Jump;


    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<PlayerMovement>();
        Jump = GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collideWall) {
            collideWall = Physics2D.OverlapCircle(wallCheckRight.position, Jump.checkRadius, WallLayer);
        }
        if (!collideWall)
        {
            collideWall = Physics2D.OverlapCircle(wallCheckLeft.position, Jump.checkRadius, WallLayer);
        }


        if (collideWall && !Jump.isGrounded && Movement.movement.x != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        if (Input.GetButtonDown("Jump") && isSliding)
        {
            isWallJumping = true;
            Invoke("SetWallJumping", wallJumpTime);
        }

    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            Movement.rb.velocity = new Vector2(Movement.rb.velocity.x, Mathf.Clamp(Movement.rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (isWallJumping)
        {
            Movement.rb.velocity = new Vector2(-wallJumpForcex * Movement.movement.x, wallJumpForcey);
        }

    }

    void SetWallJumping()
    {
        isWallJumping = false;
    }


    //Time for sliding
/*    private IEnumerator Slide()
    {
        yield return new WaitForSeconds(slideTime);
    }*/
}
