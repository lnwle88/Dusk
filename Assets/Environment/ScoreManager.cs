using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0; // Player's score
    public TextMeshProUGUI scoreText; // Reference to the TMP text on the HUD

    private float timeSinceLastKill; // Timer for last kill
    private int multiplier = 1; // Current score multiplier
    private const float multiplierTimeWindow = 2f; // Time window to achieve a multiplier (in seconds)

    public static int FinalScore { get; private set; } // Static property to store the final score

    private void Awake()
    {
        // Ensure there's only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreText();
    }

    private void Update()
    {
        // Reset the multiplier if the time since the last kill exceeds the time window
        if (timeSinceLastKill > multiplierTimeWindow)
        {
            ResetMultiplier();
        }
        else
        {
            timeSinceLastKill += Time.deltaTime; // Increment the timer
        }
    }

    // Call this method to add points to the score
    public void AddScore(int points)
    {
        score += points * multiplier; // Apply multiplier to score
        UpdateScoreText();
        timeSinceLastKill = 0f; // Reset timer after a kill
        UpdateMultiplier(); // Update the multiplier based on kills
    }

    // Update the score text in the HUD
    public void UpdateScoreText()
    {
        scoreText.text = score.ToString(); // Just show the score as a number
    }

    public void StoreFinalScore()
    {
        FinalScore = score; // Store the final score when the player dies
    }

    private void UpdateMultiplier()
    {
        if (multiplier < 2) // Increase multiplier only up to 2x
        {
            multiplier++;
        }
    }

    private void ResetMultiplier()
    {
        multiplier = 1; // Reset multiplier to 1x
    }
}
