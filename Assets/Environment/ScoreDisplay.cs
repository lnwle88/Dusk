using UnityEngine;
using TMPro; // Ensure you're using TextMeshPro

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Assign this in the inspector

    void Start()
    {
        // Display the final score
        scoreText.text = ScoreManager.FinalScore.ToString();
    }
}
