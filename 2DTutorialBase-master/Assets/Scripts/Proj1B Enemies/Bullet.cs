using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Current bug: Fireball does not work when colliding on the right side of enemies, but works in every other direction
    #region Attack_variables
    [SerializeField]
    private float damage;
    private float radius;
    [Tooltip("Indicate what type of enemy it can attack")]
    //Will be set to either "Player" (if bullet instance) or "Enemy" (if fireball instance) for flexibility of script, to know what tag to deal damage to
    public string target;
    #endregion

    #region Move_variables
    [SerializeField]
    private int moveSpeed;
    public Vector3 direction; //This will be decided and set by caster
    [SerializeField]
    [Tooltip("Indicates range at which to delete ")]
    private float range;
    #endregion

    #region BulletAttatchment_variables
    Rigidbody2D BulletRB;
    CircleCollider2D BulletCollider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        BulletRB = GetComponent<Rigidbody2D>();
        BulletCollider = GetComponent<CircleCollider2D>();

        radius = BulletCollider.radius;
    }

    private void Update()
    {
        //Move so long as object still has range
        BulletRB.velocity = direction.normalized * moveSpeed;
        range -= Time.deltaTime;
        if (range <= 0)
        {
            Destroy(this.gameObject);
        }
        
    }
    #endregion

    #region Attack_functions
    //upon collider hitting player tag, exlode and delete this object
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag(target))
        {
            Explode();
        }
    }

    //Method taken from Enemy.cs Attack region
    private void Explode()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag(target))
            {
                //Unideal, should normally have more generalized scripts for enemy types but will do for this case but will do just for this case
                if (target == "Player")    //Bullet, damage player
                {
                    hit.transform.GetComponent<PlayerController>().TakeDamage(damage);
                } else if (target == "Enemy")    //Fireball, damage enemy
                {
                    if (hit.transform.GetComponent<Enemy>())
                    {
                        hit.transform.GetComponent<Enemy>().TakeDamage(damage);
                    } else if (hit.transform.GetComponent<Turret>())
                    {
                        hit.transform.GetComponent<Turret>().TakeDamage(damage);
                    } else if (hit.transform.GetComponent<SlimeBoss>())
                    {
                        hit.transform.GetComponent<SlimeBoss>().TakeDamage(damage);
                    }
                } 
                Destroy(this.gameObject);

            }
        }
        Destroy(this.gameObject);
    }
    #endregion
}
