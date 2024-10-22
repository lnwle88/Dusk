using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RanniAI : MonoBehaviour
{
    // Ranni Follow
    public Transform player;
    public GameObject projectilePrefab; 
    public float fireCooldown = 1f; 
    private ManaSystem manaSystem; 

    private AIPath aiPath;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float nextFireTime = 0f; // Tracks when the next shot can be fired

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        manaSystem = GetComponent<ManaSystem>(); 

        aiPath.canSearch = true;
        aiPath.canMove = true;
    }

    void Update()
    {
        aiPath.destination = player.position;

        
        animator.SetFloat("Speed", aiPath.velocity.magnitude);

        // Flip the sprite based on movement direction
        if (aiPath.velocity.x > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (aiPath.velocity.x < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }

        // Fire projectiles with right mouse button
        if (Input.GetMouseButtonDown(1) && Time.time >= nextFireTime) // Right mouse button
        {
            if (manaSystem.UseMana(manaSystem.manaCostPerProjectile)) // Check if there's enough mana
            {
                FireProjectile();
                nextFireTime = Time.time + fireCooldown; // Set the next fire time
            }
            else
            {
                Debug.Log("Not enough mana to fire!"); 
            }
        }
    }

    private void FireProjectile()
    {
        // Calculate the direction towards the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // Create and shoot the projectile from the companion's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the projectile's velocity directly towards the mouse
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectile.GetComponent<Projectile>().speed; // Set projectile speed

        // Calculate the angle and set the rotation of the projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Apply rotation
    }
}
