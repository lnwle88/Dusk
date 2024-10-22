using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    private Animator animator;
    private bool isSwingingRight = true;
    private bool isSwinging = false;
    private Camera mainCamera;

    // Default rotation for the sword
    private Quaternion defaultRotation;

    // Rotation speed for smooth return
    public float rotationSpeed = 5f;

    // Damage amount
    public int swordDamage = 20;

    // Reference to the sword collider
    public CircleCollider2D swordCollider;

    // Attack cooldown variables
    public float attackCooldown = 1f; // Time in seconds
    private float lastAttackTime = 0f;

    
    private bool hasHitEnemy = false;

    // Charged attack variables
    public float maxChargeTime = 2f; // Time to fully charge the attack
    private float chargeTime = 0f;
    public bool isCharging { get; private set; } = false; 

    
    public GameObject wavePrefab; 
    public float waveSpeed = 10f; 
    public float waveLifetime = 2f; 

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        
        defaultRotation = transform.localRotation;

        // Disable the collider initially
        swordCollider.enabled = false;
    }

    void Update()
    {
        // Handle sword swing on mouse click
        if (Input.GetMouseButtonDown(0) && !isSwinging && Time.time >= lastAttackTime + attackCooldown)
        {
            // Start charging the attack
            isCharging = true;
        }

        // If the player is holding the attack button, increase charge time
        if (isCharging)
        {
            chargeTime += Time.deltaTime;

            // Release the charged attack when mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                if (chargeTime >= maxChargeTime)
                {
                    ShootWaveTowardsMouse(); // Fire charged wave attack
                }
                else
                {
                    PerformSwordSwing(); // Perform a normal sword swing if not fully charged
                }

                isCharging = false; // Reset charging state
                chargeTime = 0f; // Reset charge time
            }
        }

        // Handle sword rotation back to default
        if (!isSwinging)
        {
            SmoothReturnToDefaultRotation();
        }
    }

    private void PerformSwordSwing()
    {
        AimTowardsMouse();
        swordCollider.enabled = true; // Enable collider at the start of the swing

        if (isSwingingRight)
        {
            animator.SetTrigger("RSwing");
        }
        else
        {
            animator.SetTrigger("LSwing");
        }

        isSwingingRight = !isSwingingRight;
        isSwinging = true;
        lastAttackTime = Time.time; 
        hasHitEnemy = false; // Reset the hit flag for the new swing
    }

    private void AimTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = mousePosition - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void SmoothReturnToDefaultRotation()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultRotation, rotationSpeed * Time.deltaTime);
    }

    public void EndSwing()
    {
        Debug.Log("EndSwing called");
        isSwinging = false;
        swordCollider.enabled = false; // Disable the collider after the swing
        animator.ResetTrigger("RSwing");
        animator.ResetTrigger("LSwing");
        animator.Play("Idle");
    }

    // Method to shoot the wave towards the mouse
    private void ShootWaveTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0f; 

        Vector2 direction = (mousePosition - transform.position).normalized;

        
        GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);

        
        Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * waveSpeed; // Move the wave towards the mouse
        }

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        wave.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the wave after its lifetime expires
        Destroy(wave, waveLifetime);

        
        PerformSwordSwing();

        Debug.Log("Charged wave attack fired towards mouse!");
    }

    
    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }

    
    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;
    }

    public bool IsCharging()
    {
        return isCharging; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSwinging && !hasHitEnemy)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyAI enemy = collision.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(swordDamage);
                    hasHitEnemy = true;
                }
            }
            else if (collision.CompareTag("Breakable"))
            {
                BreakableObject breakable = collision.GetComponent<BreakableObject>();
                if (breakable != null)
                {
                    breakable.Break(); 
                    hasHitEnemy = true;
                }
            }
        }
    }
}