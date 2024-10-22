using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessModeSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f; // Time between spawns
    public Transform tilemap; // Reference to your tilemap
    private Camera mainCamera;

    private float minSpawnDistanceFromCamera = 5f; // Minimum distance from camera bounds for spawning

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector2 spawnPosition = GetRandomSpawnPositionOutsideCamera();
            if (IsWithinTilemap(spawnPosition))
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector2 GetRandomSpawnPositionOutsideCamera()
    {
        // Get the camera bounds
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Define the area outside the camera for spawning
        float spawnX;
        float spawnY;

        // Randomly decide whether to spawn on the left/right or top/bottom of the camera
        if (Random.value > 0.5f) // Spawn on left or right
        {
            spawnX = Random.Range(cameraPosition.x - cameraWidth / 2f - minSpawnDistanceFromCamera,
                                  cameraPosition.x + cameraWidth / 2f + minSpawnDistanceFromCamera);
            spawnY = Random.value > 0.5f
                ? Random.Range(cameraPosition.y - cameraHeight / 2f, cameraPosition.y + cameraHeight / 2f) // In vertical bounds
                : Random.Range(-minSpawnDistanceFromCamera, minSpawnDistanceFromCamera); // Outside vertical bounds
        }
        else // Spawn on top or bottom
        {
            spawnY = Random.Range(cameraPosition.y - cameraHeight / 2f - minSpawnDistanceFromCamera,
                                  cameraPosition.y + cameraHeight / 2f + minSpawnDistanceFromCamera);
            spawnX = Random.value > 0.5f
                ? Random.Range(cameraPosition.x - cameraWidth / 2f, cameraPosition.x + cameraWidth / 2f) // In horizontal bounds
                : Random.Range(-minSpawnDistanceFromCamera, minSpawnDistanceFromCamera); // Outside horizontal bounds
        }

        return new Vector2(spawnX, spawnY);
    }

    bool IsWithinTilemap(Vector2 position)
    {
        // Ensure the position is within the tilemap's bounds
        Bounds tilemapBounds = tilemap.GetComponent<Renderer>().bounds;
        return tilemapBounds.Contains(position);
    }
}
