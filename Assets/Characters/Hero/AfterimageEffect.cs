using System.Collections;
using UnityEngine;

public class AfterimageEffect : MonoBehaviour
{
    public float afterimageDuration = 0.5f;  // Duration the afterimage is visible
    public float fadeDuration = 0.5f;         // Time taken to fade out

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartAfterimageEffect(Vector3 position, Quaternion rotation)
    {
        // Set the position and rotation of the afterimage
        transform.position = position;
        transform.rotation = rotation;

        
        StartCoroutine(AfterimageCoroutine());
    }

    private IEnumerator AfterimageCoroutine()
    {
        // Set the afterimage to be visible
        spriteRenderer.enabled = true;

        // Wait for the afterimage duration
        yield return new WaitForSeconds(afterimageDuration);

        // Fade out effect
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Disable the sprite renderer after fading out
        spriteRenderer.enabled = false;

        // Destroy the afterimage GameObject after fading out
        Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            if (spriteRenderer != null) 
            {
                float alpha = Mathf.Lerp(startColor.a, 0, elapsedTime / fadeDuration);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }

        // Disable the sprite renderer after fading out
        if (spriteRenderer != null) // Check again before disabling
        {
            spriteRenderer.enabled = false;
        }
    }
}
