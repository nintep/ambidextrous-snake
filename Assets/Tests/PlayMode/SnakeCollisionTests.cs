using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Linq;

public class SnakeCollisionTests
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
    public IEnumerator SnakeStopsWhenCollidingWithWall()
    {
        bool snakeDied = false;
        void OnSnakeDeath()
        {
            snakeDied = true;
        }

        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        snake.SnakeDied += OnSnakeDeath;
        Assert.False(snakeDied);

        Vector3 snakeStartPos = snake.transform.position;

        PlayerMovement movement = snake.GetComponent<PlayerMovement>();
        movement.SetStartDirection(Vector2.up);
        movement.StartMovement();

        yield return null;

        // Check that snake has moved up
        Vector3 snakePos = snake.transform.position;
        Assert.Greater(snakePos.y, snakeStartPos.y);

        // Move wall in front of snake
        GameObject wall = GameObject.Find("Wall_top");
        Assert.NotNull(wall);

        wall.transform.position = snakePos + Vector3.up * 0.5f;

        yield return new WaitForSeconds(1.0f);

        // Check that snake death event is called
        Assert.True(snakeDied);

        // Check that snake is no longer moving
        snakePos = snake.transform.position;
        yield return null;
        Assert.AreEqual(snakePos, snake.transform.position);

        snake.SnakeDied -= OnSnakeDeath;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator AdjacentSnakeSegmentsHaveNoCollision()
    {
        bool snakeDied = false;
        void OnSnakeDeath()
        {
            snakeDied = true;
        }

        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        snake.SnakeDied += OnSnakeDeath;
        Assert.False(snakeDied);

        PlayerMovement movement = snake.GetComponent<PlayerMovement>();
        movement.SetStartDirection(Vector2.up);
        movement.StartMovement();

        // Add pickup item to snake's position
        PickUpItem item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;

        yield return new WaitForSeconds(0.5f);

        movement.AddMoveInput(Vector2.down);

        yield return new WaitForSeconds(0.5f);
        Assert.False(snakeDied);

        snake.SnakeDied -= OnSnakeDeath;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator SnakeCanCollideWithSelf()
    {
        bool snakeDied = false;
        void OnSnakeDeath()
        {
            snakeDied = true;
        }

        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        snake.SnakeDied += OnSnakeDeath;
        Assert.False(snakeDied);

        PlayerMovement movement = snake.GetComponent<PlayerMovement>();
        movement.SetStartDirection(Vector2.up);
        movement.StartMovement();

        // Add pickup item to snake's position to add new segment
        PickUpItem item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;

        yield return new WaitForSeconds(0.5f);

        // Add pickup item to snake's position to add new segment
        item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;

        yield return new WaitForSeconds(0.5f);

        movement.AddMoveInput(Vector2.down);

        yield return new WaitForSeconds(0.5f);
        Assert.True(snakeDied);

        // Check that snake is no longer moving
        Vector3 snakePos = snake.transform.position;
        yield return null;
        Assert.AreEqual(snakePos, snake.transform.position);

        snake.SnakeDied -= OnSnakeDeath;
    }

    [UnityTest, Timeout(5000)]
    public IEnumerator SnakeCollidesWithIncorrectPickUpType()
    {
        bool snakeDied = false;
        void OnSnakeDeath()
        {
            snakeDied = true;
        }

        Snake snake = GameObject.FindFirstObjectByType<Snake>();
        Assert.IsNotNull(snake);

        snake.SnakeDied += OnSnakeDeath;
        Assert.False(snakeDied);

        PlayerMovement movement = snake.GetComponent<PlayerMovement>();
        movement.SetStartDirection(Vector2.up);
        movement.StartMovement();

        // Add pickup item to snake's position with correct pickup type
        PickUpItem item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;
        item.SetType(snake.PlayerType == PlayerType.Left ? PickUpType.Food_left : PickUpType.Food_right);

        yield return new WaitForSeconds(0.5f);

        Assert.False(snakeDied);

        // Add pickup item to snake's position with incorrect pickup type
        item = GameObject.FindAnyObjectByType<PickUpItem>();
        Assert.NotNull(item);
        item.transform.position = snake.transform.position;
        item.SetType(snake.PlayerType == PlayerType.Left ? PickUpType.Food_right : PickUpType.Food_left);

        yield return new WaitForSeconds(0.5f);

        Assert.True(snakeDied);
        snake.SnakeDied -= OnSnakeDeath;
    }
}
