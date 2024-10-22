using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    private float timeElapsed = 0f;
    private bool isRunning = true; 

    void Start()
    {
        
        timeElapsed = 0f;
    }

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime; // Update the time

            // Format the time as minutes and seconds
            int minutes = Mathf.FloorToInt(timeElapsed / 60F);
            int seconds = Mathf.FloorToInt(timeElapsed % 60F);

            // Display the time in the TextMeshPro UI element
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isRunning = false; // Stop the timer
    }

    public void StartTimer()
    {
        isRunning = true; // Start or resume the timer
    }
}
