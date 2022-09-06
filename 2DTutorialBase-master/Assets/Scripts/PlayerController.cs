using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    float xInput;
    float yInput;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Attack_variables
    public float damage;
    public float attackSpeed = 1;
    float attackTimer;
    public float hitboxTiming;
    public float endAnimationTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();

        attackTimer = 0;

        anim = GetComponent<Animator>();

        currHealth = maxHealth;

        HPSlider.value = currHealth / maxHealth;
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        Move();

        //Attack
        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        } else
        {
            attackTimer -= Time.deltaTime;
        }

        //Interact
        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();
        }
    }
    #endregion

    #region Movement_functions
    private void Move()
    {
        anim.SetBool("Moving", true);

        if (xInput > 0)
        {
            PlayerRB.velocity = Vector2.right * moveSpeed;
            currDirection = Vector2.right;
        } else if (xInput < 0)
        {
            PlayerRB.velocity = Vector2.left * moveSpeed;
            currDirection = Vector2.left;
        } else if (yInput > 0)
        {
            PlayerRB.velocity = Vector2.up * moveSpeed;
            currDirection = Vector2.up;
        } else if (yInput < 0)
        {
            PlayerRB.velocity = Vector2.down * moveSpeed;
            currDirection = Vector2.down;
        } else
        {
            PlayerRB.velocity = Vector2.zero;
            anim.SetBool("Moving", false);
        }

        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }
    #endregion

    #region Attack_functions
    private void Attack()
    {
        Debug.Log("Attacking Now");
        Debug.Log(currDirection);
        attackTimer = attackSpeed;
        
        //Handles all attack animations hitboxes
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        anim.SetTrigger("AttackTrig");

        //start sound effect
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        yield return new WaitForSeconds(hitboxTiming);
        Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Tons of damage!");
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(hitboxTiming);
        isAttacking = false;

        yield return null;
    }
    #endregion

    #region Health_functions
    //Take damage based on value param passed in by caller
    public void TakeDamage(float value)
    {
        //call sound effect
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        //Decrement Health
        currHealth -= value;
        Debug.Log("Health is now" + currHealth.ToString());

        //change UI
        HPSlider.value = currHealth / maxHealth;

        //Check if dead
        if (currHealth <= 0)
        {
            //Die
            Die();
        }
    }

    //heals player based on value param passed in by caller
    public void Heal(float value)
    {
        //increment health by value
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        Debug.Log("Health is now" + currHealth.ToString());
        HPSlider.value = currHealth / maxHealth;
    }

    //destroys player object and triggers end
    private void Die()
    {
        //call death sound effect
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //Destroy this object
        Destroy(this.gameObject);

        //trigger anything to end the game, find GameManager and lose game
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }
    #endregion

    #region Interact_functions
    private void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }
    #endregion
}
