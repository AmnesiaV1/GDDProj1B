using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeLineOfSight : MonoBehaviour
{
    //Handles targetting player location as well as spawning enemy 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<SlimeBoss>().playerPos = collision.transform;
            GetComponentInParent<SlimeBoss>().startSpawning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<SlimeBoss>().startSpawning = false;
        }
    }
}
