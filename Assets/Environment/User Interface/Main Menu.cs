using System.Collections; 
using UnityEngine.SceneManagement; 
using UnityEngine; 
using UnityEngine.UI; 

public class MainMenu : MonoBehaviour
{
    public Image fadeImage; 
    public float fadeDuration = 1f; // Duration of the fade effect

    public enum GameMode
    {
        Endless,
        Story
    }

    public static GameMode currentGameMode; 

    private void Start()
    {
        // Make sure the fade image starts fully black
        fadeImage.color = new Color(0, 0, 0, 1); 

        // Start fading in at the beginning of the scene
        StartCoroutine(FadeIn());
    }

    public void Tutorial()
    {
        StartCoroutine(FadeAndLoadScene("Tutorial"));
    }

    public void Back()
    {
        StartCoroutine(FadeAndLoadScene("Menu"));
    }

    public void Continue()
    {
        StartCoroutine(FadeAndLoadScene("Mode"));
    }

    public void Story()
    {
        currentGameMode = GameMode.Story; // Set the game mode to Story
        StartCoroutine(FadeAndLoadScene("Level 1"));
    }

    public void Endless()
    {
        currentGameMode = GameMode.Endless; // Set the game mode to Endless
        StartCoroutine(FadeAndLoadScene("Level E"));
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeAndExitGame()); // Fade to black and quit the game
    }
    IEnumerator FadeAndExitGame()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        Debug.Log("Game is exiting...");
        Application.Quit();
    }

    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure fully black before loading the new scene
        fadeImage.color = new Color(0, 0, 0, 1);

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    
    IEnumerator FadeIn()
    {
        // Fade from black to transparent
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent after fading in
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}