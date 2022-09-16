using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Tooltip("The starting position of the enemy")]
    public float startingPos;
    [Tooltip("The distance the enemy can move away from the starting position")]
    public float dist = 5;
    [Tooltip("The movement speed of the enemy")]
    public float speed = 2f;
    [Tooltip("The direction the enemy is facing; -1 for left +1 for right")]
    public int dir;
    [Tooltip("A bool for determining whether the enemy is facing left")]
    bool facingLeft = true;
    [Tooltip("A bool for determining whether the player is in range")]
    //Enemy Vision Range is determined by a Box Collider 2D on the player GameObject
    bool playerInRange = false;
    [Tooltip("The playable character's main GameObject")]
    public GameObject player;
    [Tooltip("Th movement speed of the enemy when in charging range of player ")]
    public float chargeSpeed = 4f;
    [Tooltip("The Rigidbody2D on the Enemy GameObject")]
    public Rigidbody2D rb;
    [Tooltip("the desired height for a jump")]
    public float jumpHeight = 1f;
    //calculated jump strength needed to reach desired height 
    private float jumpForce;
    [Tooltip("A bool that ensures the enemy does his charge animation before chasing the player")]
    bool readyToChase;
    public float enemyReactionTime = 1;
    bool isGrounded = true;
    public Animator enemyAnimator;
    void Start()
    {
        //starting frame takes the enemy's starting position and their starting direction
        startingPos = transform.position.x;
        dir = -1;
        //jump calculation from https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#jump_unity
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
    }
    void Update()
    {
        if (playerInRange && readyToChase && isGrounded)
        {
            FaceTowardsPlayer();
            CheckSpriteDirection();
            //moves the enemy forward based on speed given and direction
            Debug.Log("moving toward player");
            Vector2 tempVector2 = Vector2.MoveTowards(transform.position, player.transform.position, chargeSpeed * Time.deltaTime);
            transform.position = new Vector3(tempVector2.x, transform.position.y, 0);
            enemyAnimator.SetBool("isChasing", true);
        }
        else if (!playerInRange && isGrounded)
        {
            readyToChase = false;
            //if the enemy is too far to the right from the starting position flips the enemy's direction to the left
            if (transform.position.x > startingPos + dist)
            {
                //flips the direction to the left
                dir = -1;
                Debug.Log("Moving Left");
            }
            //if the enemy is too far to the left from the starting position flips the enemy's direction to the right
            else if (transform.position.x < startingPos - dist)
            {
                //flips the direction to the right
                dir = 1;
                Debug.Log("Moving Right");
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
            Debug.Log("Flipping Sprite");
        }
        //if the enemy is moving to the right and facing the left calls the flipSprite function
        else if (dir == 1 && facingLeft)
        {
            FlipSprite();
            Debug.Log("Flipping Sprite");
        }
    }
    //Function that checks if the enemy is to the left or to the right of the player. Changes the direction accordingly.
    private void FaceTowardsPlayer()
    {
        //if enemy is to the right of player look left
        if (transform.position.x > player.transform.position.x)
        {
            dir = -1;
            Debug.Log("Looking At Player (Left)");
        }
        //if enemy is to the left of player look right
        else if (transform.position.x < player.transform.position.x)
        {
            dir = 1;
            Debug.Log("Looking At Player (Right)");
        }
    }
    //Coroutine that is called when first entering aggro range. Turns the enemy sprite to face the player, waits, jumps, waits the same amount, then is ready to begin chasing.
    IEnumerator StartCharge()
    {
        Debug.Log("Coroutine Called");
        FaceTowardsPlayer();
        CheckSpriteDirection();
        enemyAnimator.SetBool("isMoving", false);
        yield return new WaitForSeconds(enemyReactionTime);
        //adds upward force to make enemy jump
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(enemyReactionTime);
        readyToChase = true;
    }
    //When the enemy enters the aggro range the playerInRange bool is set to true and the charge coroutine is started
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //checks the triggers tag to ensure it is the aggro range
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Entering Player Range");
            playerInRange = true;
            StartCoroutine(StartCharge());
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        isGrounded = true;
    }
    //When the enemy leaves the aggro range the playerInRange bool is set to false, the readyToChase bool is set to false to allow for the charge animation to play again, and the charge coroutine is ended in case it is still running
    private void OnTriggerExit2D(Collider2D collider)
    {
        //checks the triggers tag to ensure it is the aggro range
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Exiting Player Range");
            playerInRange = false;
            readyToChase = false;
            StopAllCoroutines();
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("OnCollisionExit2D");
        isGrounded = false;
    }
}
