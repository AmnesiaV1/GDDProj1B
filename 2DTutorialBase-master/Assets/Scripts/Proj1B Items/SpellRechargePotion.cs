using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellRechargePotion : MonoBehaviour
{
    //When in collision of player:
    //1) Reduce player cast time
    //2) Change FireballCooldown slider color
    //Disappear (turn off rigidbody and sprite)
    //Do this on a timer, this buff only lasts for timer seconds (default 10)
    //Delete gameobject

    #region Spell_variables
    [SerializeField]
    [Tooltip("Indicates duration of buff")]
    private float buffTimer = 10f;
    [SerializeField]
    [Tooltip("Indicates rate at which to decrease castTimer")]
    private float timeReduce = 2f;
    private float originalCastTimer;
    #endregion

    #region Spell_functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(RechargeBuffRoutine(collision));
        }
    }

    IEnumerator RechargeBuffRoutine(Collider2D collision)
    {
        //Set new values
        originalCastTimer = collision.transform.GetComponent<PlayerController>().castSpeed;
        collision.transform.GetComponent<PlayerController>().castSpeed = timeReduce;
        collision.transform.GetComponent<PlayerController>().castTimer = 0f;
        //change fireball slider color
        collision.transform.GetComponent<PlayerController>().FireballCooldown.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.cyan;
        //Fill.color = Color.cyan;
        GetComponent<Renderer>().enabled = false;
        Debug.Log("Recharge Buff");
        yield return new WaitForSeconds(buffTimer);

        //Reset to old values
        collision.transform.GetComponent<PlayerController>().castSpeed = originalCastTimer;
        collision.transform.GetComponent<PlayerController>().FireballCooldown.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1f, 0.3247987f, 0f);
        Destroy(this.gameObject);
        yield return null;
    }
    #endregion
}
