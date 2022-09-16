using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Similar implementation to the LineOfSight script
//Will begin attacking the player when they get in range
public class TurretSensor : MonoBehaviour
{
    //In range start-up
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<Turret>().activate = true;
            Debug.Log("SEE PLAYER, ACTIVATE SPAWNER");
        }
    }

    //In range effects
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<Turret>().targetPosition = coll.GetComponent<PlayerController>().transform.position;
        }
    }

    //Out of range
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))  //If player exits range, deactivate turret
        {
            GetComponentInParent<Turret>().activate = false;
            Debug.Log("PLAYER LOCATION LOST");
        } 
    }
}
