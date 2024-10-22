using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int maxHealth = 1; // Health of the object
    public GameObject healthPotionPrefab; // Assign the health potion prefab in the Inspector
    [Range(0f, 1f)] public float dropChance = 0.5f; // 50% chance to drop

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        DropHealthPotion(); // Call method to handle potion drop

        // Destroy the object
        Destroy(gameObject);
    }

    private void DropHealthPotion()
    {
        // Check if a healthPotionPrefab is assigned
        if (healthPotionPrefab != null)
        {
            // 50% chance to drop a health potion
            if (Random.value <= dropChance)
            {
                Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Health potion prefab not assigned to BreakableObject.");
        }
    }
}
