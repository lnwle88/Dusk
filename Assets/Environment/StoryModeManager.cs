using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryModeManager : MonoBehaviour
{
    private List<EnemyAI> enemies; // Store references to enemies

    void Start()
    {
        // Find all EnemyAI components in the scene
        enemies = new List<EnemyAI>(FindObjectsOfType<EnemyAI>());
    }

    void Update()
    {
        // Check if all enemies are defeated
        if (AreAllEnemiesDefeated())
        {
            // Handle logic for when all enemies are defeated
            Debug.Log("All enemies defeated!");
            // You can load the next level or display a message here
        }
    }

    private bool AreAllEnemiesDefeated()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.currentHealth > 0) // Check if the enemy is still alive
            {
                return false; // At least one enemy is still alive
            }
        }
        return true; // All enemies are defeated
    }
}
