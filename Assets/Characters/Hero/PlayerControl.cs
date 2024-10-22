using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    // Body
    public GameObject Hero;
    public GameObject AfterImagePrefab;  // Prefab reference for afterimages

    // Animation
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    // Movement
    public float MoveSpeed = 5f;
    public float slowDownFactor = 0.5f; // Factor to slow down movement
    public float dashSpeed = 15f;
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    public float afterimageSpawnInterval = 0.1f; 
    public int afterimageCount = 5;               

    public Slider DSlider; 
    public Slider chargeSlider; 

    private float dashCounter = 0f;
    private float dashCooldownCounter = 0f;
    private bool isDashing = false;

    private bool isCharging = false; // Track whether the player is charging
    private float chargeTime = 2f; // Total charge time
    private float currentCharge = 0f; // Current charge amount

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private HeroAttack heroAttack; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        heroAttack = GetComponentInChildren<HeroAttack>(); 

        if (chargeSlider != null)
        {
            chargeSlider.gameObject.SetActive(false); 
            chargeSlider.maxValue = chargeTime; // Set max value to the charge time
            chargeSlider.value = 0f; 
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleDash();
        HandleCharging(); 
        UpdateDashCooldownSlider(); 

        // Update the slider position to follow the character without flipping
        if (chargeSlider != null && chargeSlider.gameObject.activeSelf)
        {
            Vector3 sliderPosition = transform.position; 
            sliderPosition.y += 1f; 
            chargeSlider.transform.position = sliderPosition; // Set the slider's position
        }
    }


    private void HandleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        // Debug log to ensure animator is not null
        if (animator != null)
        {
            animator.SetFloat("SpeedX", Mathf.Abs(moveInput.x));
            animator.SetFloat("SpeedY", Mathf.Abs(moveInput.y));
        }
        else
        {
            Debug.LogError("Animator is not assigned in PlayerControl!");
            return; // Exit if animator is null to prevent further errors
        }

        // Debug log to ensure rb is not null
        if (rb != null)
        {
            float currentMoveSpeed = MoveSpeed; // Default move speed

            if (heroAttack != null)
            {
                if (heroAttack.IsCharging()) // Check charging state
                {
                    currentMoveSpeed *= 0.25f; // Reduce speed to 25%
                }
            }
            else
            {
                Debug.LogError("HeroAttack component is not found in children of this GameObject!");
                return; // Exit if HeroAttack is null to prevent further errors
            }

            if (!isDashing)
            {
                rb.velocity = moveInput * currentMoveSpeed; // Use adjusted move speed
            }
            else
            {
                rb.velocity = moveInput * dashSpeed;
            }
        }
        else
        {
            Debug.LogError("Rigidbody2D is not assigned in PlayerControl!");
        }
    }

    private void HandleCharging()
    {
        // Check if the charge input is pressed
        if (Input.GetButtonDown("Fire1")) // Assuming Fire1 is your charge button
        {
            isCharging = true;
            currentCharge = 0f; // Reset current charge
            chargeSlider.gameObject.SetActive(true); // Show the slider
        }

        if (isCharging)
        {
            // Increase charge
            currentCharge += Time.deltaTime;

            // Clamp the charge to the maximum
            if (currentCharge > chargeTime)
            {
                currentCharge = chargeTime; // Prevent overflow
            }

            // Update the slider value
            chargeSlider.value = currentCharge;

            // Check if the charge input is released
            if (Input.GetButtonUp("Fire1"))
            {
                isCharging = false;
                chargeSlider.gameObject.SetActive(false); // Hide the slider again
                // Call your attack method here, e.g., PerformWaveAttack();
                PerformWaveAttack();
            }
        }
    }

    private void PerformWaveAttack()
    {
        // Implement your wave attack logic here
        // For example:
        // GameObject wave = Instantiate(wavePrefab, transform.position, Quaternion.identity);
        // WaveAttack waveAttack = wave.GetComponent<WaveAttack>();
        // waveAttack.waveDamage += Mathf.FloorToInt(currentCharge); // Increase damage based on charge time
    }

    private void HandleDash()
    {
        // Prevent dashing if charging
        if (isCharging) return;

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownCounter <= 0 && dashCounter <= 0)
        {
            isDashing = true;
            dashCounter = dashLength;

            // Start coroutine to create multiple afterimages during the dash
            StartCoroutine(CreateAfterimages());

            // Start cooldown
            dashCooldownCounter = dashCooldown;
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                isDashing = false;
            }
        }

        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }


    private IEnumerator CreateAfterimages()
    {
        for (int i = 0; i < afterimageCount; i++)
        {
            CreateAfterimage();
            yield return new WaitForSeconds(afterimageSpawnInterval);
        }
    }

    private void CreateAfterimage()
    {
        // Instantiate the afterimage at the player's position with the same rotation
        GameObject afterimage = Instantiate(AfterImagePrefab, transform.position, Quaternion.identity);
        AfterimageEffect afterimageEffect = afterimage.GetComponent<AfterimageEffect>();

        if (afterimageEffect != null)
        {
            afterimageEffect.StartAfterimageEffect(transform.position, transform.rotation);
        }

        // Set the afterimage's local scale to match the player's facing direction
        SpriteRenderer afterimageSpriteRenderer = afterimage.GetComponent<SpriteRenderer>();
        if (afterimageSpriteRenderer != null)
        {
            Vector3 localScale = afterimage.transform.localScale;
            localScale.x = transform.localScale.x; // Match player's X scale
            afterimage.transform.localScale = localScale;
        }
    }

    private void UpdateDashCooldownSlider()
    {
        DSlider.value = dashCooldownCounter / dashCooldown; // Update the slider value
    }
}