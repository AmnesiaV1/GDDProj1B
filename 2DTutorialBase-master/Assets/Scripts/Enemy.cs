using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    #endregion

    #region Targetting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Unity_functions
    //runs once on creation
    private void Awake()
    {
        EnemyRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;

    }

    //runs every frame
    private void Update()
    {
        //check to see if we know where player is
        if (player == null)
        {
            return;
        }

        Move();
    }
    #endregion

    #region Movement_variables
    //move directly at the player
    private void Move()
    {
        //Calculate movement vector player position - enemy position = direction of player relative to enemy
        Vector2 direction = player.position - transform.position;

        EnemyRB.velocity = direction.normalized * moveSpeed;
    }
    #endregion

    #region Attack
    //raycasts box for player, causes damage, spawns explosion prefab
    private void Explode()
    {
        //call audiomanager for explosion
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                //Cause damage to player
                Debug.Log("Hit Player with explosion");

                //spawn explosion prefab
                Instantiate(explosionObj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
                Destroy(this.gameObject);

            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.CompareTag("Player"))
        {
            if (coll != null)
            {
                Explode();
            }
        }
    }
    #endregion

    #region Health_functions
    //enemy takes damage based on value param
    public void TakeDamage(float value)
    {
        FindObjectOfType<AudioManager>().Play("BatHurt");

        //decrement health
        currHealth -= value;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Destroys game object
        Destroy(this.gameObject);
    }
    #endregion
}
