using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 1; // Damage dealt by the projectile

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after lifetime
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
            EnemyAI enemyAI = collision.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage); 
            }

            // Destroy the projectile on hit with an enemy
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall")) // Check if it collided with a wall
        {
            // Destroy the projectile on hit with a wall
            Destroy(gameObject);
        }
    }
}
