using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class HeroHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public HealthBar HealthBar; 
    public Animator animator; 
    public PlayerControl playerControl; 
    private Rigidbody2D rb; 
    private SpriteRenderer spriteRenderer; 

    private bool isDead = false;
    public SceneTransition sceneTransition; 

    public enum GameMode
    {
        Endless,
        Story
    }

    public GameMode currentGameMode; 

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetMaxHealth(maxHealth); // Initialize health bar to max health
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        if (isDead)
        {
            return; // Exit early if the player is dead
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return; // Prevent further damage after death

        currentHealth -= damage;
        HealthBar.SetHealth(currentHealth); // Update health bar after taking damage

        // Flash red when taking damage
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Only store the final score if in endless mode
        if (MainMenu.currentGameMode == MainMenu.GameMode.Endless)
        {
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.StoreFinalScore();
            }
        }

        isDead = true;
        animator.SetTrigger("Die"); // Trigger death animation
        DisablePlayerControls();    // Disable player controls
        StopMovement();             // Stop player movement

        // Load the death scene after a short delay
        Invoke("LoadDeathScene", 1f); 
    }

    void LoadDeathScene()
    {
        if (sceneTransition != null)
        {
            string sceneName = (currentGameMode == GameMode.Endless)
                ? "Death - Endless"  // Load Endless mode death scene
                : "Death - Story";    // Load Story mode death scene

            sceneTransition.FadeToScene(sceneName); // Transition to the appropriate death scene
        }
    }

    void DisablePlayerControls()
    {
        if (playerControl != null)
        {
            playerControl.enabled = false; // Disable PlayerControl script
        }
    }

    void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reset velocity to stop movement
        }
    }

    IEnumerator FlashRed()
    {
        // Save the original color of the sprite
        Color originalColor = spriteRenderer.color;

        // Set the sprite's color to red
        spriteRenderer.color = Color.red;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Revert the sprite's color back to its original color
        spriteRenderer.color = originalColor;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); 
        HealthBar.SetHealth(currentHealth); // Update health bar after healing
    }
}
