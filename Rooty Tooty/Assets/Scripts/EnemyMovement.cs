using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float startingPos;
    public float dist = 5;
    public float speed = 5;
    public int dir;
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
            flipSprite();
            Debug.Log("Flipping Sprite");
        }
        //if the enemy is moving to the right and facing the left calls the flipSprite function
        else if (dir == 1 && facingLeft)
        {
            flipSprite();
            Debug.Log("Flipping Sprite");
        }
        //moves the enemy forward based on speed given and direction
        transform.Translate(Vector3.right * speed * Time.deltaTime * dir);

    }
    //Flips the sprite along the x axis by multiplying the x scale by -1
    void flipSprite()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingLeft = !facingLeft;
    }
}
