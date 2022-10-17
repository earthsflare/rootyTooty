using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinotaurMovement : MonoBehaviour
{
    [Tooltip("The starting position of the charge")]
    private Vector3 chargeStartPos;
    [Tooltip("The end position of the charge")]
    private Vector3 endPos;
    [Tooltip("The duration/speed of the charge")]
    public float chargeDuration = 2f;
    [Tooltip("time variable used in lerp")]
    private float elapsedTime;
    [Tooltip("random int between 0 - 100 to determine attack")]
    private int attackProbability;
    [Tooltip("Distance that the minotaur will charge")]
    public float chargeDistance = 10;
    [Tooltip("Bool to know when enemy is charging")]
    private bool chargeAttack = false;
    [Tooltip("Bool to know when enemy can attack")]
    private bool canAttack = true;
    [Tooltip("Bool to know when enemy finishes charge")]
    private bool finishedCharge;
    [Tooltip("float that determines charge attack cooldown")]
    public float ChargeCooldown = 2;
    [Tooltip("float that determines axe attack cooldown")]
    public float AxeCooldown = 2;
    [Tooltip("float that edits the animation speed of the minotaur during its charge")]
    private float chargeAnimationSpeed = 1.5f;
    [Tooltip("The direction the enemy is facing; -1 for left +1 for right")]
    private int dir;
    [Tooltip("A bool for determining whether the enemy is facing left")]
    private bool facingLeft;
    [Tooltip("A bool for determining whether the player is in range")]
    //Enemy Vision Range is determined by a Box Collider 2D on the player GameObject
    bool inAggroRange = false;
    [Tooltip("The playable character's main GameObject")]
    public GameObject player;


    public float enemyReactionTime = 1;
    public Animator enemyAnimator;
    void Start()
    {
        dir = (int)gameObject.transform.localScale.x / Math.Abs((int)gameObject.transform.localScale.x);
        facingLeft = dir == -1;
        //starting frame takes the enemy's starting position and their starting direction
        endPos = new Vector3(transform.position.x + (chargeDistance * dir), transform.position.y, transform.position.z);
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (inAggroRange)
        {
            if (canAttack) Attack();
            if (chargeAttack)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / chargeDuration;
                transform.position = Vector3.Lerp(chargeStartPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));
            }
            if ((transform.position.x == chargeStartPos.x - chargeDistance || transform.position.x == chargeStartPos.x + chargeDistance) && !finishedCharge)
            {
                enemyAnimator.speed = 1;
                enemyAnimator.SetBool("isCharging", false);
                finishedCharge = true;
                chargeAttack = false;
                StartCoroutine(ChargeAttackCooldown());
            }
        }
    }
    //Flips the sprite along the x axis by multiplying the x scale by -1 Source: https://www.youtube.com/watch?v=Cr-j7EoM8bg
    private void FlipSprite()
    {
        transform.position = new Vector3(transform.position.x + (4 * dir), transform.position.y, transform.position.z);
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
    }
    private void ChargeAttack()
    {
        elapsedTime = 0;
        canAttack = false;
        chargeAttack = true;
        finishedCharge = false;
        chargeStartPos = transform.position;
        endPos = new Vector3(transform.position.x + (chargeDistance * dir), transform.position.y, transform.position.z);
        enemyAnimator.speed = chargeAnimationSpeed;
        enemyAnimator.SetBool("isCharging", true);
    }
    private void AxeAttack()
    {
        canAttack = false;
        enemyAnimator.Play("00minotaur - export_attack");
        StartCoroutine(AxeAttackCooldown());
    }
    private void Attack()
    {
        attackProbability = UnityEngine.Random.Range(0, 101);
        if (attackProbability > 50)
        {
            Debug.Log("Charge Attack");
            ChargeAttack();
        }
        else
        {
            Debug.Log("Axe Attack");
            AxeAttack();
        }
    }
    IEnumerator ChargeAttackCooldown()
    {
        dir *= -1;
        CheckSpriteDirection();
        yield return new WaitForSeconds(ChargeCooldown);
        canAttack = true;
    }
    IEnumerator AxeAttackCooldown()
    {
        yield return new WaitForSeconds(AxeCooldown);
        canAttack = true;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //checks the triggers tag to ensure it is the aggro range
        if (collider.CompareTag("PlayerAggroRange"))
        {
            Debug.Log("Entering Player Range");
            inAggroRange = true;
        }
    }
}
