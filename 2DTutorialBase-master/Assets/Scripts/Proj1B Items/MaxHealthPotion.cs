using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPotion : MonoBehaviour
{
    #region Health_variables
    [SerializeField]
    [Tooltip("The amount the player's health increases by")]
    private int maxHealthIncrease;
    #endregion

    #region HealBuff_functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            float maxHealth = collision.transform.GetComponent<PlayerController>().maxHealth;
            float currHealth = collision.transform.GetComponent<PlayerController>().HPSlider.value;
            collision.transform.GetComponent<PlayerController>().maxHealth = maxHealth + maxHealthIncrease;
            collision.transform.GetComponent<PlayerController>().HPSlider.value = (currHealth * maxHealth) / (maxHealth + maxHealthIncrease);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
