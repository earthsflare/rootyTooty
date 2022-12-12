using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{

    //Player Roll Variables
    private bool canRoll;
    [SerializeField] private float rollingSpd = 2f;
    [SerializeField] private float rollingTime = 1f;
    [SerializeField] private float rollingCooldown = 1f;

    int BushLayer;
    int PlayerLayer;

    [HideInInspector] PlayerMovement Movement;
    [SerializeField] private AudioSource Rollsfx;

    #region Getter Setters

    // Check if player has the roll ability
    public bool getRoll()
    {
        return canRoll;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<PlayerMovement>();

        BushLayer = LayerMask.NameToLayer("RollBlock");
        PlayerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Player Roll
        if (Input.GetButtonDown("Roll") && canRoll)
        {
            StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        canRoll = false;
        Movement.isRolling = true;
        Movement.animator.SetBool("isRolling", Movement.isRolling);

        //We don't want gravity to affect character while roll/dash in air
        float currentGravity = Movement.rb.gravityScale;
        Movement.rb.gravityScale = 0f;

        if (!Movement.facingRight)
        {
            Movement.movement.x = -1 * Movement.regSpeed;
        }
        else
        {
            Movement.movement.x = 1 * Movement.regSpeed;
        }
       
        Physics2D.IgnoreLayerCollision(BushLayer, PlayerLayer, true);
        Rollsfx.Play();
        Movement.rb.velocity = new Vector2(Movement.movement.x * rollingSpd, 0f);

        yield return new WaitForSeconds(rollingTime);
        Movement.rb.gravityScale = currentGravity;
        Movement.isRolling = false;
        Movement.animator.SetBool("isRolling", Movement.isRolling);
        Physics2D.IgnoreLayerCollision(BushLayer, PlayerLayer, false);

        yield return new WaitForSeconds(rollingCooldown);
        canRoll = true;
    }
}
