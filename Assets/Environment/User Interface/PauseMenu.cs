using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Image fadeImage; 
    public float fadeDuration = 1f; // Duration of the fade effect
    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        StartCoroutine(FadeAndLoadScene("Menu")); // Fade and load the main menu scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; 
        StartCoroutine(FadeAndQuitGame()); // Fade to black and quit the game
    }

    
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(sceneName);
    }

    
    IEnumerator FadeAndQuitGame()
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
}
