using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 1; // Amount of health restored

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HeroHealth playerHealth = collision.GetComponent<HeroHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount); // Call the Heal method to increase player's health
                Destroy(gameObject); // Destroy the health potion after picking up
            }
        }
    }
}
