using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    public int waveDamage = 30; // Damage the wave deals to enemies
    public float lifetime = 2f; // Time before the wave disappears

    private void Start()
    {
        // Destroy the wave after a set time (lifetime)
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the wave hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(waveDamage); // Deal damage to the enemy
            }
        }

        // Check if the wave hits a breakable object
        if (collision.CompareTag("Breakable"))
        {
            BreakableObject breakable = collision.GetComponent<BreakableObject>();
            if (breakable != null)
            {
                breakable.Break(); // Call the Break method on the breakable object
            }
        }
    }

}
