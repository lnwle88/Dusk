using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; 
    public float fadeDuration = 1f; // How long the fade lasts

    // Call this function to start the fade out and load the next scene
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        // Gradually increase the alpha value of the image (fade to black)
        float elapsedTime = 0f;
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        // Once fully faded, load the next scene
        SceneManager.LoadScene(sceneName);
    }
}
