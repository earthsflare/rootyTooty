using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HobbitMovement : MonoBehaviour
{
    [Tooltip("The starting position of the enemy")]
    private float startingPos;
    [Tooltip("The distance the enemy can move Left from the starting position")]
    public float distL = 5;
    [Tooltip("The distance the enemy can move Right from the starting position")]
    public float distR = 5;
    [Tooltip("The movement speed of the enemy")]
    public float speed = 2f;
    [Tooltip("The direction the enemy is facing; -1 for left +1 for right")]
    public float dir;
    [Tooltip("A bool for determining whether the enemy is facing left")]
    private bool facingLeft;
    [Tooltip("A bool for determining whether the player is in range")]
    //Enemy Vision Range is determined by a Box Collider 2D on the player GameObject
    bool inAggroRange = false;
    [Tooltip("The playable character's main GameObject")]
    public GameObject player;
    [Tooltip("Th movement speed of the enemy when in charging range of player ")]
    public float chargeSpeed = 4f;
    [Tooltip("The Rigidbody2D on the Enemy GameObject")]
    public Rigidbody2D rb;
    [Tooltip("the desired height for a jump")]
    public float jumpHeight = 0.1f;
    //calculated jump strength needed to reach desired height 
    private float jumpForce;
    private bool weaponCoolingDown = false;
    public float enemyReactionTime = 0.5f;
    public float slingChargeTime = .5f;
    public float cooldownDuration = 1.6f;
    bool isGrounded = true;
    bool readyToFire;
    bool slingReady = false;
    public Animator enemyAnimator;
    void Start()
    {
        dir = (float)gameObject.transform.localScale.x;
        dir = dir/Math.Abs(dir);
        facingLeft = dir == -1;
        //starting frame takes the enemy's starting position and their starting direction
        startingPos = transform.position.x;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        //jump calculation from https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#jump_unity
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
    }
    void Update()
    {
        if (inAggroRange && readyToFire && isGrounded)
        {
            FaceTowardsPlayer();
            enemyAnimator.SetBool("isMoving", false);
            if (!weaponCoolingDown)
            {
                Attack();
                StartCoroutine(WaitSlingCharge());
                if (slingReady)
                {
                    Debug.Log("Getting Hobbit projectile from pool");
                    // Get projectile from the projectile pool
                    GameObject projectile = HobbitProjectilePooler.hobbitProjectilePool.GetPooledObject();
                    if (projectile != null)
                    {
                        projectile.transform.position = transform.position;

                        Vector3 dir = Player.instance.transform.position - projectile.transform.position;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                        projectile.SetActive(true);
                    }
                    StartCoroutine(StartCooldown());
                }
            }
        }

        else if (!inAggroRange && isGrounded)
        {
            //if the enemy is too far to the right from the starting position flips the enemy's direction to the left
            if (transform.position.x > startingPos + distR)
            {
                //flips the direction to the left
                dir = -1;
            }
            //if the enemy is too far to the left from the starting position flips the enemy's direction to the right
            else if (transform.position.x < startingPos - distL)
            {
                //flips the direction to the right
                dir = 1;
            }
            CheckSpriteDirection();
            transform.Translate(dir * speed * Time.deltaTime * Vector3.right);
            enemyAnimator.SetBool("isMoving", true);
        }

        //moves towards player when in range

    }
    //Flips the sprite along the x axis by multiplying the x scale by -1 Source: https://www.youtube.com/watch?v=Cr-j7EoM8bg
    private void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingLeft = !facingLeft;
    }
    //Function that checks the sprites direction and flips it to the correct orientation
    private void CheckSpriteDirection()
    {
        //if the enemy is moving to the left and not facing the left calls the flipSprite function
        if (dir == -1 && !facingLeft)
        {
            FlipSprite();
        }
        //if the enemy is moving to the right and facing the left calls the flipSprite function
        else if (dir == 1 && facingLeft)
        {
            FlipSprite();
        }
    }
    //Function that checks if the enemy is to the left or to the right of the player. Changes the direction accordingly.
    private void FaceTowardsPlayer()
    {
        //if enemy is to the right of player look left
        if (transform.position.x > player.transform.position.x)
        {
            dir = -1;
        }
        //if enemy is to the left of player look right
        else if (transform.position.x < player.transform.position.x)
        {
            dir = 1;
        }
        CheckSpriteDirection();
    }
    private void Attack()
    {
        enemyAnimator.SetTrigger("Attack");
    }
    //Coroutine that is called when first entering aggro range. Turns the enemy sprite to face the player, waits, jumps, waits the same amount, then is ready to begin shooting.
    IEnumerator startShooting()
    {
        Debug.Log("Coroutine Called");
        FaceTowardsPlayer();
        CheckSpriteDirection();
        enemyAnimator.SetBool("isMoving", false);
        yield return new WaitForSeconds(enemyReactionTime);
        //adds upward force to make enemy jump
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(enemyReactionTime);
        readyToFire = true;
    }
    //When the enemy enters the aggro range the playerInRange bool is set to true and the shooting coroutine is started
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //checks the triggers tag to ensure it is the aggro range
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Entering Player Range");
            inAggroRange = true;
            StartCoroutine(startShooting());
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        if (col.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }
    //When the enemy leaves the aggro range the playerInRange bool is set to false, the readyToFire bool is set to false to allow for the shoot animation to play again, and the shoot coroutine is ended in case it is still running
    private void OnTriggerExit2D(Collider2D collider)
    {
        //checks the triggers tag to ensure it is the aggro range
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Exiting Player Range");
            inAggroRange = false;
            enemyAnimator.ResetTrigger("Attack");
            readyToFire = false;
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("OnCollisionExit2D");
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Coroutine for cooldown on enemy weapon
    private IEnumerator StartCooldown()
    {
        weaponCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        weaponCoolingDown = false;
        slingReady = false;
    }
    private IEnumerator WaitSlingCharge()
    {
        yield return new WaitForSeconds(slingChargeTime);
        slingReady = true;
    }
}
