using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;

public class TwoSnakesTests
{
    // Index of scene used for testing
    private int sceneIdx = 2;

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
    public IEnumerator SnakesCanCollideWithEachOther()
    {
        Snake[] snakes = GameObject.FindObjectsByType<Snake>(FindObjectsSortMode.InstanceID);
        Assert.AreEqual(2, snakes.Count());

        Snake leftSnake = snakes[0].PlayerType == PlayerType.Left ? snakes[0] : snakes[1];
        Snake rightSnake = snakes[0].PlayerType == PlayerType.Right ? snakes[0] : snakes[1];

        Assert.AreNotEqual(leftSnake, rightSnake);

        bool leftSnakeDied = false;
        bool rightSnakeDied = false;
        void OnSnakeDeath(PlayerType type)
        {
            if (type == PlayerType.Left) leftSnakeDied = true;
            if (type == PlayerType.Right) rightSnakeDied = true;
        }

        leftSnake.SnakeDied += () => OnSnakeDeath(PlayerType.Left);
        rightSnake.SnakeDied += () => OnSnakeDeath(PlayerType.Right);

        leftSnake.transform.position = Vector3.left;
        rightSnake.transform.position = Vector3.right;

        leftSnake.GetComponent<PlayerMovement>().SetStartDirection(Vector2.right);
        rightSnake.GetComponent<PlayerMovement>().SetStartDirection(Vector2.left);

        leftSnake.GetComponent<PlayerMovement>().StartMovement();
        rightSnake.GetComponent<PlayerMovement>().StartMovement();

        yield return null;

        Assert.False(leftSnakeDied);
        Assert.False(rightSnakeDied);

        yield return new WaitForSeconds(1.0f);

        Assert.True(leftSnakeDied);
        Assert.True(rightSnakeDied);
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator PickUpGrowsCorrectSnake()
    {
        Snake[] snakes = GameObject.FindObjectsByType<Snake>(FindObjectsSortMode.InstanceID);
        Assert.AreEqual(2, snakes.Count());

        Snake leftSnake = snakes[0].PlayerType == PlayerType.Left ? snakes[0] : snakes[1];
        Snake rightSnake = snakes[0].PlayerType == PlayerType.Right ? snakes[0] : snakes[1];

        Assert.AreNotEqual(leftSnake, rightSnake);

        SnakeSegment[] allSegments = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID);

        // Check segment counts for each snake
        Assert.AreEqual(1, allSegments.Where(seg => seg.Type == PlayerType.Left).Count());
        Assert.AreEqual(1, allSegments.Where(seg => seg.Type == PlayerType.Right).Count());

        leftSnake.transform.position = Vector3.left;
        rightSnake.transform.position = Vector3.right;

        leftSnake.GetComponent<PlayerMovement>().SetStartDirection(Vector2.up);
        rightSnake.GetComponent<PlayerMovement>().SetStartDirection(Vector2.up);

        leftSnake.GetComponent<PlayerMovement>().StartMovement();
        rightSnake.GetComponent<PlayerMovement>().StartMovement();

        // Add pickup item to left snake's position to add new segment
        PickUpItem item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = leftSnake.transform.position;

        yield return new WaitForSeconds(0.5f);

        allSegments = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID);

        // Check segment counts for each snake
        Assert.AreEqual(2, allSegments.Where(seg => seg.Type == PlayerType.Left).Count());
        Assert.AreEqual(1, allSegments.Where(seg => seg.Type == PlayerType.Right).Count());

        // Add pickup item to right snake's position to add new segment
        item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = rightSnake.transform.position;

        yield return new WaitForSeconds(0.5f);

        allSegments = GameObject.FindObjectsByType<SnakeSegment>(FindObjectsSortMode.InstanceID);

        // Check segment counts for each snake
        Assert.AreEqual(2, allSegments.Where(seg => seg.Type == PlayerType.Left).Count());
        Assert.AreEqual(2, allSegments.Where(seg => seg.Type == PlayerType.Right).Count());
    }
}
