using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinotaurMovement : MonoBehaviour
{
    [Tooltip("The starting position of the enemy")]
    private Vector3 startingPos;
    private Vector3 chargeStartPos;
    private Vector3 endPos;
    public float chargeDuration = 2f;
    private float elapsedTime;
    private int attackProbability;
    public float chargeDistance = 10;
    private bool chargeAttack = false;
    private bool axeAttack = false;
    private bool canAttack = true;
    private bool finishedCharge;
    public float ChargeCooldown = 2;
    public float AxeCooldown = 2;
    public float chargeAnimationSpeed = 1.5f;
    [Tooltip("The distance the enemy can move Left from the starting position")]
    public float distL = 5;
    [Tooltip("The distance the enemy can move Right from the starting position")]
    public float distR = 5;
    [Tooltip("The movement speed of the enemy")]
    public float speed = 2f;
    [Tooltip("The direction the enemy is facing; -1 for left +1 for right")]
    public int dir;
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
    public float jumpHeight = 1f;
    //calculated jump strength needed to reach desired height 
    private float jumpForce;
    [Tooltip("A bool that ensures the enemy does his charge animation before chasing the player")]
    bool readyToChase;
    public float enemyReactionTime = 1;
    bool isGrounded = true;
    bool inMeleeRange = false;
    public Animator enemyAnimator;
    void Start()
    {
        dir = (int)gameObject.transform.localScale.x / Math.Abs((int)gameObject.transform.localScale.x);
        facingLeft = dir == -1;
        //starting frame takes the enemy's starting position and their starting direction
        startingPos = transform.position;
        chargeStartPos = transform.position;
        endPos = new Vector3(transform.position.x + (chargeDistance*dir), transform.position.y, transform.position.z);
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        //jump calculation from https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/#jump_unity
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));

    }
    void Update()
    {
        if (canAttack) Attack();
        if (chargeAttack)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / chargeDuration;
            transform.position = Vector3.Lerp(startingPos, endPos, Mathf.SmoothStep(0, 1, percentageComplete));
        }
        if ((transform.position.x == startingPos.x - chargeDistance || transform.position.x == startingPos.x + chargeDistance) && !finishedCharge)
        {
            enemyAnimator.speed = 1;
            enemyAnimator.SetBool("isCharging", false);
            finishedCharge = true;
            chargeAttack = false;
            StartCoroutine(ChargeAttackCooldown());
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
        startingPos = transform.position;
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
}
