using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class EnemyAI : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float detectionRadius = 5f;
    public float fieldOfViewAngle = 180f;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackCooldown = 1f;

    public int currentHealth;
    public Transform player;

    private bool isAttacking = false;
    private bool playerInSight = false;
    private Animator animator;

    private AIPath aiPath; 
    private AIDestinationSetter destinationSetter; 
    private bool facingRight = true;

    public GameObject attackIndicatorPrefab;
    private GameObject attackIndicator;

    
    public GameObject healthPotionPrefab; 
    public float dropChance = 0.5f; // 50% chance to drop a health potion

    
    public bool isStoryMode = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>(); 
        destinationSetter = GetComponent<AIDestinationSetter>(); 
        currentHealth = maxHealth;

        // Automatically find the player by tag
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        }

        // Set the destination for A* pathfinding
        if (player != null)
        {
            destinationSetter.target = player; // Set the player as the target
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if player is within detection radius and in sight
        playerInSight = distanceToPlayer <= detectionRadius && IsPlayerInSight();

        if (playerInSight)
        {
            // Enable pathfinding to follow player
            aiPath.canMove = true;

            FlipTowardsPlayer();

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }

            
            animator.SetFloat("Speed", aiPath.velocity.magnitude);
        }
        else
        {
            // Stop moving if player is out of detection radius
            aiPath.canMove = false;
            animator.SetFloat("Speed", 0f);
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Create and position the attack indicator
        if (attackIndicatorPrefab != null)
        {
            attackIndicator = Instantiate(attackIndicatorPrefab, transform.position, Quaternion.identity);
            PositionAttackIndicator();
            attackIndicator.transform.SetParent(transform);
        }

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackCooldown);

        HeroHealth playerHealth = player.GetComponent<HeroHealth>();
        if (playerHealth != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        if (attackIndicator != null)
        {
            Destroy(attackIndicator);
        }

        isAttacking = false;
    }

    void PositionAttackIndicator()
    {
        if (attackIndicator == null) return;
        attackIndicator.transform.position = transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if current health is less than or equal to zero
        if (currentHealth <= 0)
        {
            // Call your custom die handling logic here instead of animator trigger
            HandleDeath();
        }
        else
        {
            // Flash red and step back only when taking damage
            StartCoroutine(FlashRed());
            StartCoroutine(StepBack());
        }
    }

    private void HandleDeath()
    {
        // Disable collider and movement
        GetComponent<Collider2D>().enabled = false;
        aiPath.canMove = false;

        DropHealthPotion(); // Drop a health potion on death

        // Check if the score manager is not null and if it's not story mode
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null && !isStoryMode)
        {
            scoreManager.AddScore(100); // Example: Add 100 points on kill
        }

        Destroy(gameObject, 1f); // Destroy the enemy game object after 1 second
    }

    void DropHealthPotion()
    {
        if (Random.value <= dropChance) // Check drop chance
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity); // Instantiate health potion
        }
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color flashColor = Color.red;

        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator StepBack()
    {
        Vector2 originalPosition = transform.position;
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 stepBackPosition = originalPosition - directionToPlayer * 0.5f;

        float elapsedTime = 0f;
        float stepBackDuration = 0.2f;

        while (elapsedTime < stepBackDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, stepBackPosition, elapsedTime / stepBackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < stepBackDuration)
        {
            transform.position = Vector2.Lerp(stepBackPosition, originalPosition, elapsedTime / stepBackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    bool IsPlayerInSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer);

        if (angleToPlayer <= fieldOfViewAngle / 2f)
        {
            LayerMask layerMask = LayerMask.GetMask("Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, layerMask);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        bool playerOnRight = player.position.x > transform.position.x;

        if (playerOnRight && !facingRight)
        {
            Flip();
        }
        else if (!playerOnRight && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
