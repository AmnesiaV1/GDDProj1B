using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Uses Enemy.cs as reference
public class Turret : MonoBehaviour
{
    #region Health_variables
    private float health;
    #endregion

    #region Attack_variables
    [SerializeField]
    [Tooltip("Indicates rate of fire")]
    private float attackSpeed;
    private float attackTimer;
    public GameObject ammo;
    #endregion

    #region Targetting_variables
    public bool activate;
    public Vector3 targetPosition;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        attackTimer = attackSpeed;
        activate = false;
    }

    private void Update()
    {
        if (activate)
        {
            if (attackTimer <= 0)
            {
                Attack();
            } else
            {
                attackTimer -= Time.deltaTime;
            }
        } else
        {
            attackTimer = attackSpeed;
        }
           
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    #endregion

    #region Attack_funtions
    //Spawns bullet instances in the direction of the player and launches them in that direction
    public void Attack()
    {
        //Instantiate bullet in specified direction
        Vector3 targetDirection = targetPosition - transform.position;
        GameObject bullet = Instantiate(ammo, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().direction = targetDirection;
        //reset attack timer
        attackTimer = attackSpeed;
    }
    #endregion
}
