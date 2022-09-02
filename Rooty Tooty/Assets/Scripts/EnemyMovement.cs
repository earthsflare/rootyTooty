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
    //Enemy Vision Range is determined by a Circle Collider 2D on the player GameObject
    bool playerInRange = false;
    public GameObject player;
    public float inRangeSpeed = 4f;
    public Rigidbody2D rb;
    public float jumpHeight = 1f;
    float jumpForce;
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
        if (playerInRange)
        {
            //moves the enemy forward based on speed given and direction
            Debug.Log("moving toward player");
            Vector2 tempVector2 = Vector2.MoveTowards(transform.position, player.transform.position, inRangeSpeed * Time.deltaTime);
            transform.position = new Vector3(tempVector2.x, tempVector2.y, 0);
            if (transform.position.x > player.transform.position.x)
            {
                dir = -1;
                Debug.Log("Chasing Player Left");
            }
            else if (transform.position.x < player.transform.position.x)
            {
                dir = 1;
                Debug.Log("Chasing Player Right");
            }
            SpriteDirectionCheck();
        }
        else
        {
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
            SpriteDirectionCheck();
            transform.Translate(dir * speed * Time.deltaTime * Vector3.right);
        }

        //moves towards player when in range

    }
    //Flips the sprite along the x axis by multiplying the x scale by -1
    private void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingLeft = !facingLeft;
    }
    private void SpriteDirectionCheck()
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
    IEnumerator StartCharge()
    {
        Debug.Log("Coroutine Called");
        yield return new WaitForSeconds(5);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(5);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerAggroRange")){
            Debug.Log("Entering Player Range");
            playerInRange = true;
            StartCoroutine(StartCharge());
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Exiting Player Range");
            playerInRange = false;
        }
    }
}
