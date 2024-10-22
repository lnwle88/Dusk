using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's transform
    public float parallaxEffectMultiplier; // Adjust this value for the desired effect

    private Vector3 lastCameraPosition;

    void Start()
    {
        // Store the initial camera position
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        // Calculate the distance the camera has moved
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;

        // Move the background based on the camera's movement and the parallax multiplier
        transform.position += new Vector3(deltaX * parallaxEffectMultiplier, 0, 0);

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;
    }
}
