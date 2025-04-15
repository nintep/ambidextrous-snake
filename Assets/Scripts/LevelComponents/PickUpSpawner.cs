using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnRate;

    [SerializeField]
    private PickUpItem pickUpItemPrefab;

    [SerializeField]
    private SpriteRenderer spawnArea;

    [SerializeField]
    private bool startAutomatically;

    private bool paused;
    private float timeUntilNextSpawn;
    private int numRetries = 5;
    private float minDistanceBetweenPickups = 0.5f;

    // Store bounds of the spawn area
    private Vector2 minBound;
    private Vector2 maxBound;

    private void Start()
    {
        SetSpawningActive(startAutomatically);
        Bounds bounds = spawnArea.bounds;
        maxBound = bounds.max;
        minBound = bounds.min;
        spawnArea.gameObject.SetActive(false);        
    }

    private void Update()
    {
        if (paused) return;

        timeUntilNextSpawn -= Time.deltaTime;
        timeUntilNextSpawn = Math.Max(timeUntilNextSpawn, 0);

        if (timeUntilNextSpawn <= 0)
        {
            timeUntilNextSpawn = 1 / spawnRate;
            SpawnRandomPickUp();
        }
    }

    private void SpawnRandomPickUp()
    {
        // Try to find an empty position to spawn pickup
        (bool, Vector2) spawnPosition = GetEmptySpawnPosition();
        if (spawnPosition.Item1)
        {
            PickUpItem pickUp = GameObject.Instantiate(pickUpItemPrefab);
            pickUp.transform.parent = transform;

            int typeIdx = UnityEngine.Random.Range(0, 3);
            PickUpType type = typeIdx == 0 ? PickUpType.Food
                            : (typeIdx == 1 ? PickUpType.Food_left 
                            : PickUpType.Food_right);
            pickUp.SetType(type);
            
            pickUp.transform.position = spawnPosition.Item2;        
        }
    }

    private (bool, Vector2) GetEmptySpawnPosition()
    {
        // Try to find an empty position at most numRetries times
        for (int i = 0; i < numRetries; i++)
        {
            Vector2 position = GetRandomPositionInSpawnArea();

            // Check if position overlaps with a collider
            if (!Physics2D.OverlapCircle(position, minDistanceBetweenPickups))
            {
                return (true, position);
            }
        }

        // If empty position was not found, return 0
        return (false, Vector2.zero);
    }

    private Vector2 GetRandomPositionInSpawnArea()
    {
        float x = UnityEngine.Random.Range(minBound.x, maxBound.x);
        float y = UnityEngine.Random.Range(minBound.y, maxBound.y);
        return new Vector2(x, y);
    }


    public void SetSpawningActive(bool shouldSpawn)
    {
        paused = !shouldSpawn;
        timeUntilNextSpawn = 1 / spawnRate;
    }
}
