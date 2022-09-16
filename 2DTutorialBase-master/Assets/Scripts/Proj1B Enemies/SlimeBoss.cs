using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
    //Main implementations of slime boss:
    //1) Will follow the player at a very slow speed and deal damage upon contact (without exploding)
    //2) Will very periodically spawn enemy ghosts 

    #region Health_variables
    [SerializeField]
    private float maxHealth;
    private float currHealth;
    #endregion

    #region Attack_variables
    //Contact damage
    [SerializeField]
    private float contactDamage;
    private float contactRadius;

    //Spawn enemies
    public GameObject enemyPrefab;
    [SerializeField]
    private float enemySpawnSpeed;
    private float enemySpawnTimer;
    public bool startSpawning;
    #endregion

    #region Move_variables
    [SerializeField]
    private float moveSpeed;
    Rigidbody2D SlimeRB;
    public Transform playerPos;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        currHealth = maxHealth;
        enemySpawnTimer = enemySpawnSpeed;
        startSpawning = false;
        SlimeRB = GetComponent<Rigidbody2D>();
        contactRadius = GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        Move();

        if (startSpawning)
        {
            if (enemySpawnTimer <= 0)
            {
                SpawnEnemy();
            }
            else
            {
                enemySpawnTimer -= Time.deltaTime;
            }
        }
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region Move_functions
    private void Move()
    {
        if (playerPos == null)
        {
            return;
        }

        Vector2 direction = playerPos.position - transform.position;
        SlimeRB.velocity = direction.normalized * moveSpeed;
    }
    #endregion

    #region Attack_functions
    private void ContactDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, contactRadius + 1, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PlayerController>().TakeDamage(contactDamage);
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position - new Vector3(0, -1, 0), transform.rotation);
        enemySpawnTimer = enemySpawnSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            ContactDamage();
        }
    }
    #endregion
}
