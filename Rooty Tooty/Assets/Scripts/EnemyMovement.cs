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
    public float speed = 5;
    [Tooltip("The direction the enemy is facing; -1 for left +1 for right")]
    public int dir;
    [Tooltip("A bool for determining whether the enemy is facing left")]
    bool facingLeft = true;
    void Start()
    {
        //starting frame takes the enemy's starting position and their starting direction
        startingPos = transform.position.x;
        dir = -1;
    }
    void Update()
    {
        //if the enemy is too far to the right from the starting position flips the enemy's direction to the left
        if (transform.position.x > startingPos + dist)
        {
            //flips the direction to the left
            dir = -1;
            Debug.Log("Moving Left");
        }
        else
        {
            //if the enemy is too far to the left from the starting position flips the enemy's direction to the right
            if (transform.position.x < startingPos - dist)
            {
                //flips the direction to the right
                dir = 1;
                Debug.Log("Moving Right");
            }
        }
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
        //moves the enemy forward based on speed given and direction
        transform.Translate(dir * speed * Time.deltaTime * Vector3.right);

    }
    //Flips the sprite along the x axis by multiplying the x scale by -1
    void FlipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingLeft = !facingLeft;
    }
}
