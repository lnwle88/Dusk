using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HeroHealth heroHealth; // Reference to HeroHealth component

    // Call this method to start Endless Mode
    public void StartEndlessMode()
    {
        // Set the current game mode to Endless
        heroHealth.currentGameMode = HeroHealth.GameMode.Endless;

        // Optionally, you can initialize or reset other components for Endless Mode here
        // For example: Reset the score, set up enemies, etc.

        Debug.Log("Endless Mode Started");
    }

    // Call this method to start Story Mode
    public void StartStoryMode()
    {
        // Set the current game mode to Story
        heroHealth.currentGameMode = HeroHealth.GameMode.Story;

        // Optionally, you can initialize or reset other components for Story Mode here
        // For example: Load a specific level, set initial score, etc.

        Debug.Log("Story Mode Started");
    }

    // You can also add a method to reset the game or return to the main menu
}
