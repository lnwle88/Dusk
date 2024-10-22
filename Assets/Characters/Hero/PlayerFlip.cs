using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    // Player Flip
    private float XInput;
    private bool FacingRight = true;

    // Reference to the sword's transform
    private Transform swordTransform;

    // Start is called before the first frame update
    void Start()
    {
        
        GameObject sword = GameObject.Find("HeroSword");
        if (sword != null)
        {
            swordTransform = sword.transform;
        }
        else
        {
            Debug.LogError("HeroSword GameObject not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Flip Input
        XInput = Input.GetAxis("Horizontal");

        SetupDirectionByScale();
    }

    private void SetupDirectionByScale()
    {
        if (XInput < 0 && FacingRight || XInput > 0 && !FacingRight)
        {
            FacingRight = !FacingRight;

            // Flip player
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;

            // Flip sword if the swordTransform is assigned
            if (swordTransform != null)
            {
                Vector3 swordScale = swordTransform.localScale;
                swordScale.x *= -1;
                swordTransform.localScale = swordScale;
            }
        }
    }
}

