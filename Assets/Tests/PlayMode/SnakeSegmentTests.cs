using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;

public class SnakeSegmentTests
{
    // Index of scene used for testing
    private int sceneIdx = 1;

    [UnitySetUp]
    public IEnumerator Setup() {
        SceneManager.LoadScene(sceneIdx, LoadSceneMode.Single);
        while (SceneManager.GetActiveScene().buildIndex != sceneIdx)
        {
            yield return null;
        }

        // A few extra frames for setup
        yield return null;
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown() {
        yield return null;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator SnakeStartsWithHeadSegment()
    {
        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        SnakeSegment[] segments = snake.GetComponentsInChildren<SnakeSegment>();
        Assert.AreEqual(1, segments.Count());

        yield return null;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator SnakeGrowsOnPickupCollection()
    {
        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        // Get number of initial snake segments
        int segmenCount = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID).Count();
        Assert.AreEqual(1, segmenCount);
                
        // Add item to snake's position
        PickUpItem item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;

        PlayerMovement movement = snake.GetComponent<PlayerMovement>();
        movement.StartMovement();

        // Wait for new segment to be added
        yield return new WaitForSeconds(1f);

        segmenCount = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID).Count();
        Assert.AreEqual(2, segmenCount);

        item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;

        yield return new WaitForSeconds(1f);

        segmenCount = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID).Count();
        Assert.AreEqual(3, segmenCount);
    }
}
